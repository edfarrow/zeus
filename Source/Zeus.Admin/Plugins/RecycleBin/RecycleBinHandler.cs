using System;
using System.Linq;
using MongoDB.Bson;
using Zeus.Admin.RecycleBin;
using Zeus.Security;
using Zeus.Web;

namespace Zeus.Admin.Plugins.RecycleBin
{
	/// <summary>
	/// Can throw and restore items. Thrown items are moved to a trash 
	/// container item.
	/// </summary>
	public class RecycleBinHandler : IRecycleBinHandler
	{
		public const string FormerName = "FormerName";
		public const string FormerParent = "FormerParent";
		public const string FormerParentTitle = "FormerParentTitle";
		public const string FormerExpires = "FormerExpires";
		public const string DeletedDate = "DeletedDate";
		private readonly IHost _host;

		public RecycleBinHandler(IHost host)
		{
			_host = host;
		}

		/// <summary>Occurs before an item is thrown.</summary>
		public event EventHandler<CancelItemEventArgs> ItemThrowing;

		/// <summary>Occurs after an item has been thrown.</summary>
		public event EventHandler<ItemEventArgs> ItemThrowed;

		/// <summary>The container of thrown items.</summary>
		IRecycleBin IRecycleBinHandler.TrashContainer
		{
			get { return GetTrashContainer(true); }
		}

		public RecycleBinContainer GetTrashContainer(bool create)
		{
			ContentItem rootItem = ContentItem.Find(_host.CurrentSite.RootItemID);
			RecycleBinContainer trashContainer = rootItem.GetChild("RecycleBin") as RecycleBinContainer;
			if (create && trashContainer == null)
			{
				trashContainer = new RecycleBinContainer { Parent = rootItem };
				trashContainer.Name = "RecycleBin";
				trashContainer.Title = "Recycle Bin";
				trashContainer.Visible = false;
				//trashContainer.AuthorizationRules.Add(new AuthorizationRule(trashContainer, "admin"));
				//trashContainer.AuthorizationRules.Add(new AuthorizationRule(trashContainer, "Editors"));
				//trashContainer.AuthorizationRules.Add(new AuthorizationRule(trashContainer, "Administrators"));
				trashContainer.Position = int.MaxValue - 1000000;
				trashContainer.Save();
			}
			return trashContainer;
		}

		/// <summary>Checks if the trash is enabled, the item is not already thrown and for the NotThrowable attribute.</summary>
		/// <param name="affectedItem">The item to check.</param>
		/// <returns>True if the item may be thrown.</returns>
		public bool CanThrow(ContentItem affectedItem)
		{
			RecycleBinContainer trash = GetTrashContainer(false);
			bool enabled = trash == null || trash.Enabled;
			bool alreadyThrown = IsInTrash(affectedItem);
			bool throwable = affectedItem.GetType().GetCustomAttributes(typeof(NotThrowableAttribute), true).Length == 0;
			return enabled && !alreadyThrown && throwable;
		}

		/// <summary>Throws an item in a way that it later may be restored to it's original location at a later stage.</summary>
		/// <param name="item">The item to throw.</param>
		public virtual void Throw(ContentItem item)
		{
			CancelItemEventArgs args = Invoke(ItemThrowing, new CancelItemEventArgs(item));
			if (!args.Cancel)
			{
				item = args.AffectedItem;

				ExpireTrashedItem(item);
				item.Parent = GetTrashContainer(true);

				try
				{
					item.Save();
				}
				catch (PermissionDeniedException ex)
				{
					throw new PermissionDeniedException("Permission denied while moving item to trash. Try disabling security checks using Zeus.Context.Security or preventing items from being moved to the trash with the [NonThrowable] attribute", ex);
				}

				Invoke(ItemThrowed, new ItemEventArgs(item));
			}
		}

		/// <summary>Expires an item that has been thrown so that it's not accessible to external users.</summary>
		/// <param name="item">The item to restore.</param>
		public virtual void ExpireTrashedItem(ContentItem item)
		{
			item[FormerName] = item.Name;
			item[FormerParent] = item.Parent;
			item[FormerParentTitle] = item.Parent.Title;
			item[FormerExpires] = item.Expires;
			item[DeletedDate] = DateTime.Now;
			item.Expires = DateTime.Now;
			item.Name = item.ID.ToString();

			foreach (ContentItem child in item.Children)
				ExpireTrashedItem(child);
		}

		/// <summary>Restores an item to the original location.</summary>
		/// <param name="item">The item to restore.</param>
		public virtual void Restore(ContentItem item)
		{
			ContentItem parent = ContentItem.Find((ObjectId) item[FormerParent]);
			RestoreValues(item);
			item.Parent = parent;
			item.Save();
		}

		/// <summary>Removes expiry date and metadata set during throwing.</summary>
		/// <param name="item">The item to reset.</param>
		public virtual void RestoreValues(ContentItem item)
		{
			item.Name = (string) item[FormerName];
			item.Expires = (DateTime?) item[FormerExpires];

			item[FormerName] = null;
			item[FormerParent] = null;
			item[FormerParentTitle] = null;
			item[FormerExpires] = null;
			item[DeletedDate] = null;

			foreach (ContentItem child in item.Children)
				RestoreValues(child);
		}

		/// <summary>Determines wether an item has been thrown away.</summary>
		/// <param name="item">The item to check.</param>
		/// <returns>True if the item is in the scraps.</returns>
		public bool IsInTrash(ContentItem item)
		{
			RecycleBinContainer trash = GetTrashContainer(false);
			return trash != null && item.DescendantsAndSelf.Contains(trash);
		}

		protected virtual T Invoke<T>(EventHandler<T> handler, T args)
				where T : ItemEventArgs
		{
			if (handler != null)
				handler.Invoke(this, args);
			return args;
		}
	}
}