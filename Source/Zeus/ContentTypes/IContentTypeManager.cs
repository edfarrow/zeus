using System;
using System.Collections.Generic;
using System.Security.Principal;
using Zeus.EditableTypes;

namespace Zeus.ContentTypes
{
	public interface IEditableTypeManager
	{
		ICollection<EditableType> GetEditableTypes();
		EditableType GetEditableType(object value);
		EditableType GetEditableType(Type type);
	}

	public interface IContentTypeManager
	{
		ContentItem CreateInstance(Type itemType, ContentItem parentItem);

		ICollection<ContentType> GetContentTypes();
		ContentType GetContentType(string discriminator);
		ContentType GetContentType(ContentItem item);
		ContentType GetContentType(Type type);
		IList<ContentType> GetAllowedChildren(ContentType contentType, IPrincipal user);
	}
}