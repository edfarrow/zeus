using System;
using System.Collections.Generic;
using System.Linq;
using Zeus.BaseLibrary.Reflection;
using Zeus.Design.Editors;

namespace Zeus.ContentTypes
{
	public class ContentTypeBuilder : IContentTypeBuilder
	{
		#region Fields

		private readonly ITypeFinder _typeFinder;
		private readonly IEditableHierarchyBuilder<IEditor> _hierarchyBuilder;
		private readonly AttributeExplorer<IEditor> _editableExplorer;
		private readonly AttributeExplorer<IEditorContainer> _containableExplorer;

		#endregion

		#region Constructor

		public ContentTypeBuilder(ITypeFinder typeFinder, 
			IEditableHierarchyBuilder<IEditor> hierarchyBuilder,
			AttributeExplorer<IEditor> editableExplorer,
			AttributeExplorer<IEditorContainer> containableExplorer)
		{
			_typeFinder = typeFinder;
			_hierarchyBuilder = hierarchyBuilder;
			_editableExplorer = editableExplorer;
			_containableExplorer = containableExplorer;
		}

		#endregion

		#region Methods

		public IDictionary<Type, ContentType> GetDefinitions()
		{
			IList<ContentType> definitions = FindDefinitions();
			ExecuteRefiners(definitions);
			return definitions.ToDictionary(ct => ct.ItemType);
		}

		private IList<ContentType> FindDefinitions()
		{
			// Find definitions.
			List<ContentType> definitions = new List<ContentType>();
			foreach (Type type in EnumerateTypes())
			{
				var itemDefinition = new ContentType(type);

				var editors = _editableExplorer.Find(itemDefinition.ItemType).ToList();
				editors.Sort();
				itemDefinition.Editors = editors;

				itemDefinition.Containers = _containableExplorer.Find(itemDefinition.ItemType);

				itemDefinition.RootContainer = _hierarchyBuilder.Build(itemDefinition.Containers, editors);
				definitions.Add(itemDefinition);
			}
			definitions.Sort();

			return definitions;
		}

		protected void ExecuteRefiners(IList<ContentType> definitions)
		{
			foreach (ContentType definition in definitions)
				foreach (IDefinitionRefiner refiner in definition.ItemType.GetCustomAttributes(typeof(IDefinitionRefiner), false))
					refiner.Refine(definition, definitions);
			foreach (ContentType definition in definitions)
				foreach (IInheritableDefinitionRefiner refiner in definition.ItemType.GetCustomAttributes(typeof(IInheritableDefinitionRefiner), true))
					refiner.Refine(definition, definitions);
		}

		private IEnumerable<Type> EnumerateTypes()
		{
			return _typeFinder.Find(typeof (ContentItem)).Where(t => !t.IsAbstract);
		}

		#endregion
	}
}