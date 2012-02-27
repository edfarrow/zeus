using System;
using System.Web.UI;
using Zeus.ContentTypes;
using Zeus.Editors.Attributes;

namespace Zeus.Web.UI.WebControls
{
	public class ItemEditView : ItemView
	{
		private bool _postedBack;

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
				editor.UpdateEditor(CurrentItem, PropertyControls[editor.Name]);
		}

		public void Save(ContentItem item)
		{
			EnsureChildControls();
			Zeus.Context.AdminManager.Save(item, PropertyControls, Page.User);
		}
	}
}