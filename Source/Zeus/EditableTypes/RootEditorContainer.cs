using System.Web.UI;

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