using System;
using Zeus.BaseLibrary.Web.UI;
using Zeus.ContentTypes;
using Zeus.EditableTypes;
using Zeus.Editors.Attributes;
using Zeus.Web.UI;

namespace Zeus.Templates.ContentTypes
{
	[TabPanel("Content", "Content", 10)]
	[DefaultContainer("Content")]
	public abstract class BaseContentItem : ContentItem
	{
		[TextBoxEditor("Title", 10, Required = true, Shared = false)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		public override bool IsPage
		{
			get { return false; }
		}

		protected virtual string GetIconUrl(Type type, string resourceName)
		{
			return WebResourceUtility.GetUrl(type, resourceName);
		}
	}
}