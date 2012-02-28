using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.EditableTypes;
using Zeus.Editors.Attributes;

namespace Zeus.Editors.Controls
{
	public class ItemEditor : WebControl, INamingContainer
	{
		private EditableType _currentEditableType;
		private IEditableObject _currentItem;
		private bool _postedBack;

		#region Properties

		public IDictionary<string, Control> PropertyControls { get; private set; }

		/// <summary>Gets or sets the item to edit with this form.</summary>
		public virtual IEditableObject CurrentItem
		{
			get { return _currentItem; }
			set
			{
				_currentItem = value;
				if (value != null)
				{
					ChildControlsCreated = false;
					EnsureChildControls();
				}
			}
		}

		public EditableType CurrentEditableType
		{
			get { return _currentEditableType ?? (_currentEditableType = Zeus.Context.EditableTypes.GetEditableType(CurrentItem)); }
			set { _currentEditableType = value; }
		}

		#endregion

		#region Methods

		protected override void CreateChildControls()
		{
			// Get ItemDefinition for current type.
			PropertyControls = new Dictionary<string, Control>();
			AddPropertyControls();

			base.CreateChildControls();
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
				foreach (IContainable subContained in ((IEditorContainer)contained).GetContained(Page.User))
					AddPropertyControlsRecursive(addedControl, subContained);
		}

		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState(savedState);
			_postedBack = true;
		}

		protected virtual void AddPropertyControls()
		{
			if (CurrentEditableType != null)
			{
				// Add editors and containers recursively.
				AddPropertyControlsRecursive(this, CurrentEditableType.RootContainer);

				if (!_postedBack)
					UpdateEditors();
			}
		}

		private void UpdateEditors()
		{
			foreach (IEditor editor in CurrentEditableType.GetEditors(Page.User))
				editor.UpdateEditor(CurrentItem, PropertyControls[editor.Name]);
		}

		public bool UpdateItem()
		{
			EnsureChildControls();

			bool updated = false;
			foreach (IEditor e in CurrentEditableType.GetEditors(Page.User))
				if (PropertyControls.ContainsKey(e.Name))
					updated = e.UpdateItem(CurrentItem, PropertyControls[e.Name]) || updated;
			return updated;
		}

		#endregion
	}
}
