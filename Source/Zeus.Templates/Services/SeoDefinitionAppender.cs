using Ninject;
using Zeus.ContentTypes;
using Zeus.EditableTypes;
using Zeus.Editors.Attributes;
using Zeus.Templates.Configuration;
using Zeus.Templates.ContentTypes;
using Zeus.Web;

namespace Zeus.Templates.Services
{
	public class SeoDefinitionAppender : IInitializable
	{
		private readonly IContentTypeManager _contentTypeManager;
		private readonly IEditableTypeManager _editableTypeManager;
		private readonly TemplatesSection _templatesConfig;

		public SeoDefinitionAppender(IContentTypeManager contentTypeManager, IEditableTypeManager editableTypeManager, TemplatesSection templatesConfig)
		{
			_contentTypeManager = contentTypeManager;
			_editableTypeManager = editableTypeManager;
			_templatesConfig = templatesConfig;

			HtmlTitleTitle = "HTML Title";
			MetaKeywordsTitle = "Meta Keywords";
			MetaDescriptionTitle = "Meta Description";
			SeoTabTitle = "SEO";
		}

		public string HtmlTitleTitle { get; set; }
		public string MetaKeywordsTitle { get; set; }
		public string MetaDescriptionTitle { get; set; }
		public string SeoTabTitle { get; set; }

		#region IInitializable Members

		public void Initialize()
		{
			if (_templatesConfig.Seo == null || !_templatesConfig.Seo.Enabled)
				return;

			foreach (var editableType in _editableTypeManager.GetEditableTypes())
			{
				ContentType contentType = _contentTypeManager.GetContentType(editableType.ItemType);
				if (contentType == null)
					continue;
				if (IsPage(contentType))
				{
					FieldSetAttribute seoTab = new FieldSetAttribute("SEO", SeoTabTitle, 15)
					{
						Collapsible = true,
						Collapsed = true
					};

					editableType.Add(seoTab);

					AddEditableText(editableType, HtmlTitleTitle, SeoUtility.HTML_TITLE, 11, _templatesConfig.Seo.HtmlTitleFormat, "Used in the &lt;title&gt; element on the page", 200, false);
					AddEditableText(editableType, MetaKeywordsTitle, SeoUtility.META_KEYWORDS, 21, _templatesConfig.Seo.MetaKeywordsFormat, null, 400, false);
					AddEditableText(editableType, MetaDescriptionTitle, SeoUtility.META_DESCRIPTION, 22, _templatesConfig.Seo.MetaDescriptionFormat, null, 1000, true);
				}
                else if (contentType.IgnoreSEOAssets)
                {
                    FieldSetAttribute seoTab = new FieldSetAttribute("SEO", contentType.IgnoreSEOExplanation, 15)
                    {
                        Collapsible = true,
                        Collapsed = false
                    };
					editableType.Add(seoTab);
                }
			}
		}

		private static void AddEditableText(EditableType editableType, string title, string name, int sortOrder, string formatString, string description, int maxLength, bool multiline)
		{
			var editor = new ReactiveTextBoxEditorAttribute(title, sortOrder, formatString)
			{
				Name = name,
				ContainerName = "SEO",
				MaxLength = maxLength,
				Description = description,
				Shared = false,
				PropertyType = typeof(string)
			};
			if (multiline)
				editor.TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine;
			editableType.Add(editor);
		}

		private static bool IsPage(ContentType contentType)
		{
			return (typeof(BasePage).IsAssignableFrom(contentType.ItemType)
				&& contentType.ItemType != typeof(Redirect)
                && contentType.IsPage && !contentType.IgnoreSEOAssets) ||
                typeof(WebsiteNode).IsAssignableFrom(contentType.ItemType)
                && contentType.ItemType != typeof(Redirect)
                && contentType.IsPage && !contentType.IgnoreSEOAssets;
		}

		#endregion
	}
}