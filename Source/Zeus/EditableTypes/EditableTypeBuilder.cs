using System;
using System.Collections.Generic;
using System.Linq;
using Zeus.BaseLibrary.Reflection;
using Zeus.Editors.Attributes;

namespace Zeus.EditableTypes
{
	public class EditableTypeBuilder : IEditableTypeBuilder
	{
		#region Fields

		private readonly ITypeFinder _typeFinder;
		private readonly IEditableHierarchyBuilder<IEditor> _hierarchyBuilder;
		private readonly AttributeExplorer<IEditor> _editorExplorer;
		private readonly AttributeExplorer<IEditorContainer> _containableExplorer;

		#endregion

		#region Constructor

		public EditableTypeBuilder(ITypeFinder typeFinder,
			IEditableHierarchyBuilder<IEditor> hierarchyBuilder,
			AttributeExplorer<IEditor> editorExplorer,
			AttributeExplorer<IEditorContainer> containableExplorer)
		{
			_typeFinder = typeFinder;
			_hierarchyBuilder = hierarchyBuilder;
			_editorExplorer = editorExplorer;
			_containableExplorer = containableExplorer;
		}

		#endregion

		#region Methods

		public IDictionary<Type, EditableType> GetEditableTypes()
		{
			var editableTypes = FindEditableTypes();
			ExecuteRefiners(editableTypes);
			return editableTypes.ToDictionary(ct => ct.ItemType);
		}

		private IList<EditableType> FindEditableTypes()
		{
			var editableTypes = new List<EditableType>();
			foreach (Type type in EnumerateTypes())
			{
				var editableType = new EditableType(type);

				var editors = _editorExplorer.Find(editableType.ItemType).ToList();
				editors.Sort();
				editableType.Editors = editors;

				editableType.Containers = _containableExplorer.Find(editableType.ItemType);
				editableType.RootContainer = _hierarchyBuilder.Build(editableType.Containers, editors);

				editableTypes.Add(editableType);
			}

			return editableTypes;
		}

		protected void ExecuteRefiners(IList<EditableType> editableTypes)
		{
			foreach (var editableType in editableTypes)
				foreach (IInheritableEditableTypeRefiner refiner in editableType.ItemType.GetCustomAttributes(typeof(IInheritableEditableTypeRefiner), true))
					refiner.Refine(editableType, editableTypes);
		}

		private IEnumerable<Type> EnumerateTypes()
		{
			return _typeFinder.Find(typeof(IEditableObject)).Where(t => !t.IsAbstract);
		}

		#endregion
	}
}