using System;
using System.Web.UI;
using Zeus.ContentTypes;

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

		protected override void UpdateEditorInternal(ContentItem item, Control editor)
		{
			throw new NotImplementedException();
		}
	}
}