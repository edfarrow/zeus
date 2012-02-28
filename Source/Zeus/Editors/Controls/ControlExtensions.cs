using System.Web.UI;
using Zeus.EditableTypes;
using Zeus.Web.UI;

namespace Zeus.Editors.Controls
{
	public static class ControlExtensions
	{
		public static ContentItem FindCurrentItem(this Control control)
		{
			var container = FindParent<IContentItemContainer>(control.Parent);
			if (container != null)
				return container.CurrentItem;
			
			return null;
		}

		public static IEditableObject FindCurrentEditableObject(this Control control)
		{
			var container = FindParent<ItemEditor>(control.Parent);
			if (container != null)
				return container.CurrentItem;

			return null;
		}

		public static T FindParent<T>(this Control control)
			where T : class
		{
			if (control == null || control is T)
				return control as T;
			else
				return FindParent<T>(control.Parent);
		}
	}
}
