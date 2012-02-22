﻿using Zeus.Design.Editors;

namespace Zeus.Tests.Definitions.Items
{
	[ContentType("Text page", "websiteNode")]
	public class TestTextPage : ContentItem
	{
		[HtmlTextBoxEditor("Text", 100)]
		public virtual string Text { get; set; }
	}
}
