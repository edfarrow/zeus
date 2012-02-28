using System;
using System.Collections.Generic;
using Zeus.EditableTypes;

namespace Zeus.Editors.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class DefaultContainerAttribute : Attribute, IInheritableEditableTypeRefiner
	{
		public DefaultContainerAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; set; }

		public void Refine(EditableType currentEditableType, IList<EditableType> allEditableTypes)
		{
			var hierarchyBuilder = Context.Current.Resolve<IEditableHierarchyBuilder<IEditor>>();
			bool updated = false;
			foreach (IEditor editor in currentEditableType.Editors)
				if (string.IsNullOrEmpty(editor.ContainerName))
				{
					editor.ContainerName = Name;
					updated = true;
				}
			if (updated)
				currentEditableType.RootContainer = hierarchyBuilder.Build(
					currentEditableType.Containers, 
					currentEditableType.Editors);
		}
	}
}