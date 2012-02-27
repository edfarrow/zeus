using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Zeus.Editors.Attributes;
using Zeus.Web;

namespace Zeus.ContentTypes
{
	public class ContentType : EditableType, IComparable<ContentType>, ITypeDefinition
	{
		#region Fields

		private readonly IList<ContentType> _allowedChildren = new List<ContentType>();

		#endregion

		#region Properties

		/// <summary>Gets or sets additional child types allowed below this item.</summary>
		public IList<ContentType> AllowedChildren
		{
			get { return _allowedChildren; }
		}

		public string Discriminator
		{
			get { return ContentTypeAttribute.Name ?? ItemType.Name; }
		}

		public int SortOrder
		{
			get { return ContentTypeAttribute.SortOrder; }
		}

		public string IconUrl
		{
			get { return ((ContentItem) Activator.CreateInstance(ItemType)).IconUrl; }
		}

		public ContentTypeAttribute ContentTypeAttribute { get; set; }

		/// <summary>Definitions which are not enabled are not available when creating new items.</summary>
		public bool Enabled { get; set; }

        public bool IsPage { get; set; }

        public bool IgnoreSEOAssets
        {
            get
            {
                if (typeof(PageContentItem).IsAssignableFrom(ItemType))
                    return ((PageContentItem)Activator.CreateInstance(ItemType)).UseProgrammableSEOAssets;
                else
                    return false;
            }
        }
        public string IgnoreSEOExplanation {
            get
            {
				if (typeof(PageContentItem).IsAssignableFrom(ItemType))
                    return ((PageContentItem)Activator.CreateInstance(ItemType)).UseProgrammableSEOAssetsExplanation;
                else
                    return string.Empty;
            }
        }

		/// <summary>Gets or sets whether this content type has been defined. Weirdly enough a content type
		/// may exist without being defined. To define a content type the class must implement 
		/// the <see cref="ContentType"/> attribute.</summary>
		public bool IsDefined { get; internal set; }

		/// <summary>Gets roles or users allowed to edit items defined by this content type.</summary>
		public IList<string> AuthorizedRoles { get; internal set; }

		/// <summary>Gets the name used when presenting this item class to editors.</summary>
		public string Title
		{
			get { return ContentTypeAttribute.Title; }
		}

		public AdminSiteTreeVisibility Visibility { get; set; }

		#endregion

		#region Constructor

		public ContentType(Type itemType)
		{
			Enabled = true;
			ItemType = itemType;
			ContentTypeAttribute = new ContentTypeAttribute { Title = itemType.Name, Name = itemType.Name };
			Visibility = AdminSiteTreeVisibility.Visible;
		}

		#endregion

		#region Methods

		/// <summary>Adds an allowed child definition to the list of allowed definitions.</summary>
		/// <param name="definition">The allowed child definition to add.</param>
		public void AddAllowedChild(ContentType definition)
		{
			if (!AllowedChildren.Contains(definition))
				AllowedChildren.Add(definition);
		}

		public bool IsAuthorized(IPrincipal user)
		{
			if (user == null || AuthorizedRoles == null)
				return true;
			foreach (string role in AuthorizedRoles)
				if (string.Equals(user.Identity.Name, role, StringComparison.OrdinalIgnoreCase) || user.IsInRole(role))
					return true;
			return false;
		}

		/// <summary>Find out if this item allows sub-items of a certain type.</summary>
		/// <param name="child">The item that should be checked whether it is allowed below this item.</param>
		/// <returns>True if the specified child item is allowed below this item.</returns>
		public bool IsChildAllowed(ContentType child)
		{
			return AllowedChildren.Contains(child);
		}

		/// <summary>Removes an allowed child definition from the list of allowed definitions if not already removed.</summary>
		/// <param name="definition">The definition to remove.</param>
		public void RemoveAllowedChild(ContentType definition)
		{
			if (AllowedChildren.Contains(definition))
				AllowedChildren.Remove(definition);
		}

		#region IComparable

		int IComparable<ContentType>.CompareTo(ContentType other)
		{
			return SortOrder - other.SortOrder;
		}

		#endregion

		#endregion
	}
}