using System.Web.UI;
using Zeus.Editors.Attributes;

namespace Zeus.EditableTypes
{
	internal class RootEditorContainer : EditorContainerAttribute
	{
		public RootEditorContainer()
			: base("Root", 0)
		{
		}

		public override Control AddTo(Control container)
		{
			return container;
		}
	}
}