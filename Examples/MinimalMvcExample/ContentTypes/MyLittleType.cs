using Zeus.Editors.Attributes;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
	public class MyLittleType : EmbeddedItem
	{
		[TextBoxEditor("Test String", 10)]
		public virtual string TestString { get; set; }

        /*
		[XhtmlStringContentProperty("Test Rich String", 35)]
		public virtual string TestRichString
		{
			get { return GetDetail("TestRichString", string.Empty); }
			set { SetDetail("TestRichString", value); }
		}
         */

		[TextAreaEditor("Multi Line Textbox", 35, Height = 200, Width = 500)]
		public virtual string MultiTextBox { get; set; }
	}
}