using Ninject;
using Zeus.ContentTypes;
using Zeus.Editors.Attributes;
using Zeus.Templates.Configuration;
using Zeus.Templates.ContentTypes;

namespace Zeus.Templates.Services
{
	public class TaggingDefinitionAppender : IInitializable
	{
		private readonly IContentTypeManager _contentTypeManager;
		private readonly IEditableTypeManager _editableTypeManager;
		private readonly TemplatesSection _templatesConfig;

		public TaggingDefinitionAppender(IContentTypeManager contentTypeManager, 
			IEditableTypeManager editableTypeManager, 
			TemplatesSection templatesConfig)
		{
			_contentTypeManager = contentTypeManager;
			_editableTypeManager = editableTypeManager;
			_templatesConfig = templatesConfig;
		}

		#region IInitializable Members

		public void Initialize()
		{
			if (_templatesConfig.Tagging == null || !_templatesConfig.Tagging.Enabled)
				return;

			foreach (var editableType in _editableTypeManager.GetEditableTypes())
			{
				ContentType contentType = _contentTypeManager.GetContentType(editableType.ItemType);
				if (IsPage(contentType))
				{
					var tagEditable = new LinkedItemsCheckBoxListEditorAttribute();
					tagEditable.Name = "Tags";
					tagEditable.Title = "Tags";
					tagEditable.SortOrder = 500;
					tagEditable.TypeFilter = typeof(Tag);
					tagEditable.ContainerName = "Content";

					editableType.Add(tagEditable);
				}
			}
		}

		private static bool IsPage(ContentType contentType)
		{
			return typeof(BasePage).IsAssignableFrom(contentType.ItemType)
				&& contentType.ItemType != typeof(Redirect)
				&& contentType.ItemType != typeof(Tag)
				&& contentType.ItemType != typeof(TagGroup);
		}

		#endregion
	}
}