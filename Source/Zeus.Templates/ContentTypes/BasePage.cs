﻿using Zeus.Design.Editors;

namespace Zeus.Templates.ContentTypes
{
	[DefaultTemplate]
	public abstract class BasePage : BaseContentItem
	{
		[ContentProperty("HTML Title", 11, Description = "Used in the &lt;title&gt; element on the page")]
		public virtual string HtmlTitle
		{
			get { return GetDetail("HtmlTitle", string.Empty); }
			set { SetDetail("HtmlTitle", value); }
		}

		[ContentProperty("Page Title", 12, Description = "Used in the &lt;h1&gt; element on the page")]
		public virtual string PageTitle
		{
			get { return GetDetail("PageTitle", string.Empty); }
			set { SetDetail("PageTitle", value); }
		}

		[NameEditor("URL", 20, Required = true)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		[ContentProperty("Meta Keywords", 21)]
		public virtual string MetaKeywords
		{
			get { return GetDetail("MetaKeywords", string.Empty); }
			set { SetDetail("MetaKeywords", value); }
		}

		[ContentProperty("Meta Description", 22)]
		public virtual string MetaDescription
		{
			get { return GetDetail("MetaDescription", string.Empty); }
			set { SetDetail("MetaDescription", value); }
		}

		[ContentProperty("Visible in Menu", 25)]
		public override bool Visible
		{
			get { return base.Visible; }
			set { base.Visible = value; }
		}

		protected override string IconName
		{
			get { return "page"; }
		}

		public override bool IsPage
		{
			get { return true; }
		}
	}
}