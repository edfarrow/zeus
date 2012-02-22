﻿using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using Zeus.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	public abstract class BaseDetailCollectionEditorAttribute : AbstractEditorAttribute
	{
		protected BaseDetailCollectionEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		public override bool UpdateItem(ContentItem item, Control editor)
		{
			IList detailCollection = item[Name] as IList;
			BaseDetailCollectionEditor detailCollectionEditor = (BaseDetailCollectionEditor) editor;

			List<object> propertyDataToDelete = new List<object>();

			// First pass saves or creates items.
			for (int i = 0; i < detailCollectionEditor.Editors.Count; i++)
			{
				if (!detailCollectionEditor.DeletedIndexes.Contains(i))
				{
					object existingDetail = (detailCollection.Count > i) ? detailCollection[i] : null;
					object newDetail;
					CreateOrUpdateDetailCollectionItem((ContentItem) item, existingDetail, detailCollectionEditor.Editors[i], out newDetail);
					if (newDetail != null)
						if (detailCollection.Count > i)
							detailCollection[i] = newDetail;
						else
							detailCollection.Add(newDetail);
				}
				else
				{
					propertyDataToDelete.Add(detailCollection[i]);
				}
			}

			// Do a second pass to delete the items, this is so we don't mess with the indices on the first pass.
			foreach (var propertyData in propertyDataToDelete)
				detailCollection.Remove(propertyData);

			return detailCollectionEditor.DeletedIndexes.Count > 0 || detailCollectionEditor.AddedEditors;
		}

		protected abstract void CreateOrUpdateDetailCollectionItem(ContentItem item, object existingDetail, Control editor, out object newDetail);
		protected abstract BaseDetailCollectionEditor CreateEditor();

		protected override Control AddEditor(Control container)
		{
			BaseDetailCollectionEditor detailCollectionEditor = CreateEditor();
			detailCollectionEditor.ID = Name;
			container.Controls.Add(detailCollectionEditor);
			return detailCollectionEditor;
		}

		protected override void UpdateEditorInternal(ContentItem item, Control editor)
		{
			BaseDetailCollectionEditor detailCollectionEditor = (BaseDetailCollectionEditor) editor;
			IEnumerable detailCollection = item[Name] as IEnumerable ?? new List<object>();
			detailCollectionEditor.Initialize(detailCollection);
		}
	}
}