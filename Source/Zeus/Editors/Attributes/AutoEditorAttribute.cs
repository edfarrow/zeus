using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zeus.ContentTypes;
using Zeus.FileSystem;
using Zeus.FileSystem.Images;
using Zeus.Security;

namespace Zeus.Editors.Attributes
{
	public class AutoEditorAttribute : Attribute, IEditorAttribute, ISecurable, IPropertyAwareAttribute
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
				{ typeof(decimal), () => new TextBoxEditorAttribute() },
				{ typeof(DateTime), () => new DateEditorAttribute() },
				{ typeof(TimeSpan), () => new TimeEditorAttribute() },
				{ typeof(ContentItem), () => new LinkedItemDropDownListEditor() },
				{ typeof(EmbeddedCroppedImage), () => new EmbeddedCroppedImageEditorAttribute() },
				{ typeof(EmbeddedFile), () => new EmbeddedFileEditorAttribute() },
				{ typeof(EmbeddedItem), () => new EmbeddedItemEditorAttribute() }
			};
		}
 
		public string[] AuthorizedRoles { get; set; }
		public string ContainerName { get; set; }
		public string Name { get; set; }
		public int SortOrder { get; set; }
		public string Title { get; set; }
		public PropertyInfo UnderlyingProperty { get; set; }

		public AutoEditorAttribute(string title, int sortOrder)
		{
			Title = title;
			SortOrder = sortOrder;
		}

		public IEditor GetEditor()
		{
			var knownType = Editors.Keys.FirstOrDefault(type => type.IsAssignableFrom(UnderlyingProperty.PropertyType));
			if (knownType == null)
				throw new InvalidOperationException("No default editor for property type '" + UnderlyingProperty.PropertyType + "'");

			var editor = Editors[knownType]();

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