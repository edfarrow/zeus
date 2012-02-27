using System;
using System.Web.UI;
using Zeus.ContentTypes;
using Zeus.EditableTypes;

namespace Zeus.Editors.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class EmbeddedDocumentEditorAttribute : AbstractEditorAttribute
	{
		#region Constructors

		public EmbeddedDocumentEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{

		}

		#endregion

		protected override Control AddEditor(Control container)
		{
			throw new NotImplementedException();
		}

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			throw new NotImplementedException();
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			throw new NotImplementedException();
		}
	}
}