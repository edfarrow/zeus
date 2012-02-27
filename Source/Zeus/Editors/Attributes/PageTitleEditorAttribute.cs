using Zeus.ContentTypes;
using Zeus.EditableTypes;

namespace Zeus.Editors.Attributes
{
	public class PageTitleEditorAttribute : ReactiveTextBoxEditorAttribute
	{
		public PageTitleEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder, "{Title}")
		{
			MaxLength = 200;
		}

		public PageTitleEditorAttribute()
			: base("{Title}")
		{
			MaxLength = 200;
		}

		protected override void UpdateEditorInternal(IEditableObject item, System.Web.UI.Control editor)
		{
			if (!Context.ContentTypes.GetContentType((ContentItem) item).IsPage)
				editor.Parent.Visible = false;

			base.UpdateEditorInternal(item, editor);
		}
	}
}