using System;
using NUnit.Framework;
using Zeus.Editors.Attributes;

namespace Zeus.Tests.Editors.Attributes
{
	[TestFixture]
	public class AutoEditorAttributeTests
	{
		private class MyPage : ContentItem
		{
			public bool Available { get; set; }
			public string Description { get; set; }
			public int Num { get; set; }
			public float Num2 { get; set; }
			public double Num3 { get; set; }
			public decimal Num4 { get; set; }
			public DateTime Date { get; set; }
			public TimeSpan Time { get; set; }
			public DataContentItem AnotherContentItem { get; set; }
		}

		[Test]
		[TestCase("Available", typeof(CheckBoxEditorAttribute))]
		[TestCase("Description", typeof(TextBoxEditorAttribute))]
		[TestCase("Num", typeof(TextBoxEditorAttribute))]
		[TestCase("Num2", typeof(TextBoxEditorAttribute))]
		[TestCase("Num3", typeof(TextBoxEditorAttribute))]
		[TestCase("Num4", typeof(TextBoxEditorAttribute))]
		[TestCase("Date", typeof(DateEditorAttribute))]
		[TestCase("Time", typeof(TimeEditorAttribute))]
		[TestCase("AnotherContentItem", typeof(LinkedItemDropDownListEditor))]
		public void CanGetAutoEditorForPropertyTypes(string propertyName, Type editorType)
		{
			// Arrange.
			var autoEditor = new AutoEditorAttribute
			{
				UnderlyingProperty = typeof(MyPage).GetProperty(propertyName)
			};

			// Act.
			var editor = autoEditor.GetEditor();

			// Assert.
			Assert.That(editor, Is.InstanceOf(editorType));
		}
	}
}