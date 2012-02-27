using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Zeus.ContentTypes
{
	public class ContentTypeManager : IContentTypeManager
	{
		#region Fields

		private readonly IDictionary<Type, ContentType> _contentTypes;

		#endregion

		#region Constructor

		public ContentTypeManager(IContentTypeBuilder contentTypeBuilder)
		{
			_contentTypes = contentTypeBuilder.GetContentTypes();

			// Verify that content types have unique names.
			var discriminators = new List<string>();
			foreach (var contentType in _contentTypes.Values)
			{
				if (discriminators.Contains(contentType.Discriminator))
					throw new ZeusException("Duplicate content type discriminator. The discriminator '{0}' is already in use.", contentType.Discriminator);
				discriminators.Add(contentType.Discriminator);
			}
		}

		#endregion

		#region Methods

		/// <summary>Creates an instance of a certain type of item. It's good practice to create new items through this method so the item's dependencies can be injected by the engine.</summary>
		/// <returns>A new instance of an item.</returns>
		public ContentItem CreateInstance(Type itemType, ContentItem parentItem)
		{
			ContentItem item = (ContentItem) Activator.CreateInstance(itemType);
			item.Parent = parentItem;
			return item;
		}

		public ICollection<ContentType> GetContentTypes()
		{
			return _contentTypes.Values;
		}

		public ContentType GetContentType(ContentItem item)
		{
			return GetContentType(item.GetUnderlyingType());
		}

		public ContentType GetContentType(Type type)
		{
			if (_contentTypes.ContainsKey(type))
				return _contentTypes[type];
			return null;
		}

		public IList<ContentType> GetAllowedChildren(ContentType contentType, IPrincipal user)
		{
			List<ContentType> allowedChildren = new List<ContentType>();
			foreach (ContentType childItem in contentType.AllowedChildren)
			{
				if (!childItem.IsDefined)
					continue;
				if (!childItem.Enabled)
					continue;
				if (!childItem.IsAuthorized(user))
					continue;
				allowedChildren.Add(childItem);
			}
			allowedChildren.Sort();
			return allowedChildren;
		}

		public ContentType GetContentType(string discriminator)
		{
			return _contentTypes.Values.SingleOrDefault(ct => ct.Discriminator == discriminator);
		}

		#endregion
	}
}