using System;
using System.Web.UI;
using Zeus.ContentTypes;
using Zeus.Editors.Attributes;

namespace Zeus.Web.UI.WebControls
{
	public class ItemEditView : ItemView
	{
		private bool _postedBack;

		#region Events

		public event EventHandler<ItemViewEditableObjectEventArgs> Saving;
		public event EventHandler<ItemViewEditableObjectEventArgs> Saved;

		#endregion

		protected override void AddPropertyControls()
		{
			if (CurrentItemDefinition != null)
			{
				// Add editors and containers recursively.
				AddPropertyControlsRecursive(this, CurrentItemDefinition.RootContainer);

				if (!_postedBack)
					UpdateEditors();
			}
		}

		protected override void OnInit(EventArgs e)
		{
			Page.InitComplete += OnPageInitComplete;
			base.OnInit(e);
		}

		private void OnPageInitComplete(object sender, EventArgs e)
		{
			if (_postedBack)
				EnsureChildControls();
		}

		private void AddPropertyControlsRecursive(Control control, IContainable contained)
		{            
            Control addedControl = contained.AddTo(control);
            
            if (contained is IEditor)
				PropertyControls.Add(contained.Name, addedControl);
			if (contained is IEditorContainer)
				foreach (IContainable subContained in ((IEditorContainer) contained).GetContained(Page.User))
					AddPropertyControlsRecursive(addedControl, subContained);
		}

		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState(savedState);
			_postedBack = true;
		}

		private void UpdateEditors()
		{
            foreach (IEditor editor in CurrentItemDefinition.GetEditors(Page.User))
            {
                editor.UpdateEditor(CurrentItem, PropertyControls[editor.Name]);
            }
		}

		public ContentItem Save(ContentItem item)
		{
			EnsureChildControls();

			item = Zeus.Context.AdminManager.Save(item, PropertyControls, Page.User,
				c => OnSaving(new ItemViewEditableObjectEventArgs(c)));

			OnSaved(new ItemViewEditableObjectEventArgs(CurrentItem));
			
			return item;
		}

		/// <summary>Saves <see cref="CurrentItem"/> with the values entered in the form.</summary>
		/// <returns>The saved item.</returns>
		public ContentItem Save()
		{
			CurrentItem = Save((ContentItem) CurrentItem);
			return CurrentItem;
		}

		/// <summary>Updates the <see cref="CurrentItem"/> with the values entered in the form without saving it.</summary>
		public void Update()
		{
			Zeus.Context.AdminManager.UpdateItem((ContentItem) CurrentItem, PropertyControls, Page.User);
		}

		protected virtual void OnSaving(ItemViewEditableObjectEventArgs args)
		{
			if (Saving != null)
				Saving(this, args);
		}

		protected virtual void OnSaved(ItemViewEditableObjectEventArgs args)
		{
			if (Saved != null)
				Saved(this, args);
		}
	}
}