using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using Zeus.EditableTypes;
using Zeus.Editors.Controls;

namespace Zeus.Editors.Attributes
{
	public abstract class EmbeddedCollectionEditorAttributeBase : AbstractEditorAttribute
	{
		protected EmbeddedCollectionEditorAttributeBase(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			var collection = item[Name] as IList;
			var collectionEditor = (EmbeddedCollectionEditorBase) editor;
			if (collection == null)
				item[Name] = collection = (IList) Activator.CreateInstance(UnderlyingProperty.PropertyType);

			var toDelete = new List<object>();

			// First pass saves or creates items.
			for (int i = 0; i < collectionEditor.Editors.Count; i++)
			{
				if (!collectionEditor.DeletedIndexes.Contains(i))
				{
					object existingValue = (collection.Count > i) ? collection[i] : null;
					object newValue;
					CreateOrUpdateItem(item, existingValue, collectionEditor.Editors[i], out newValue);
					if (newValue != null)
						if (collection.Count > i)
							collection[i] = newValue;
						else
							collection.Add(newValue);
				}
				else
				{
					toDelete.Add(collection[i]);
				}
			}

			// Do a second pass to delete the items, this is so we don't mess with the indices on the first pass.
			foreach (var value in toDelete)
				collection.Remove(value);

			return collectionEditor.DeletedIndexes.Count > 0 || collectionEditor.AddedEditors;
		}

		protected abstract void CreateOrUpdateItem(IEditableObject item, object existingValue, Control editor, out object newValue);

		protected override Control AddEditor(Control container)
		{
			EmbeddedCollectionEditorBase editor = CreateEditor();
			editor.ID = Name;
			container.Controls.Add(editor);
			return editor;
		}

		protected abstract EmbeddedCollectionEditorBase CreateEditor();

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			var collectionEditor = (EmbeddedCollectionEditorBase) editor;
			var collection = item[Name] as IEnumerable ?? new List<object>();
			collectionEditor.Initialize(collection);
		}
	}
}