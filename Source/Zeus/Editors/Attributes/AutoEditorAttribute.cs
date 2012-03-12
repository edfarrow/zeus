using System;
using System.Collections.Generic;
using System.Reflection;
using Zeus.ContentTypes;
using Zeus.Security;

namespace Zeus.Editors.Attributes
{
	public class AutoEditorAttribute : IEditorAttribute, ISecurable, IPropertyAwareAttribute
	{
		private static readonly Dictionary<Type, Func<AbstractEditorAttribute>> Editors;

		static AutoEditorAttribute()
		{
			Editors = new Dictionary<Type, Func<AbstractEditorAttribute>>
			{
				{ typeof(bool), () => new CheckBoxEditorAttribute() },
				{ typeof(string), () => new TextBoxEditorAttribute() },
				{ typeof(int), () => new TextBoxEditorAttribute() },
				{ typeof(float), () => new TextBoxEditorAttribute() },
				{ typeof(double), () => new TextBoxEditorAttribute() },
				{ typeof(decimal), () => new TextBoxEditorAttribute() }
			};
		}
 
		public string[] AuthorizedRoles { get; set; }
		public string ContainerName { get; set; }
		public string Name { get; set; }
		public int SortOrder { get; set; }
		public string Title { get; set; }
		public PropertyInfo UnderlyingProperty { get; set; }

		public IEditor GetEditor()
		{
			if (!Editors.ContainsKey(UnderlyingProperty.PropertyType))
				throw new InvalidOperationException("No default editor for property type '" + UnderlyingProperty.PropertyType + "'");

			var editor = Editors[UnderlyingProperty.PropertyType]();

			editor.AuthorizedRoles = AuthorizedRoles;
			editor.ContainerName = ContainerName;
			editor.Name = Name;
			editor.SortOrder = SortOrder;
			editor.Title = Title;
			editor.UnderlyingProperty = UnderlyingProperty;

			return editor;
		}
	}
}