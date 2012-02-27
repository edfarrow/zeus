using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.ContentTypes;
using Zeus.EditableTypes;
using Zeus.Editors.Attributes;

namespace Zeus.Web.UI.WebControls
{
	public class ItemEditView : WebControl, INamingContainer, ContentItemEditor
	{
		private EditableType _currentEditableType;
		private ContentItem _currentItem;
		private bool _postedBack;

		public event EventHandler<ItemViewEditableObjectEventArgs> ItemCreating;
		public event EventHandler<ItemEditViewEditableTypeEventArgs> DefinitionCreating;

		#region Properties

		public IDictionary<string, Control> PropertyControls
		{
			get;
			private set;
		}

		/// <summary>Gets or sets the item to edit with this form.</summary>
		public virtual ContentItem CurrentItem
		{
			get
			{
				if (_currentItem == null)
				{
					var args = new ItemViewEditableObjectEventArgs(null);
					OnItemCreating(args);
					_currentItem = args.AffectedItem;
				}
				return _currentItem;
			}
			set
			{
				_currentItem = value;
				if (value != null)
				{
					ChildControlsCreated = false;
					EnsureChildControls();
					OnCurrentItemChanged(EventArgs.Empty);
				}
			}
		}

		protected virtual void OnItemCreating(ItemViewEditableObjectEventArgs args)
		{
			if (ItemCreating != null)
				ItemCreating(this, args);
		}

		protected virtual void OnDefinitionCreating(ItemEditViewEditableTypeEventArgs args)
		{
			if (DefinitionCreating != null)
				DefinitionCreating(this, args);
		}

		protected virtual void OnCurrentItemChanged(EventArgs eventArgs)
		{
			
		}

		public EditableType CurrentEditableType
		{
			get
			{
				if (_currentEditableType == null)
				{
					if (CurrentItem != null)
					{
						_currentEditableType = Zeus.Context.EditableTypes.GetEditableType(CurrentItem);
					}
					else
					{
						var args = new ItemEditViewEditableTypeEventArgs(null);
						OnDefinitionCreating(args);
						_currentEditableType = args.EditableType;
					}
				}
				return _currentEditableType;
			}
			set
			{
				_currentEditableType = value;
			}
		}

		#endregion

		#region Methods

		/*protected override void OnLoad(EventArgs e)
		{
			EnsureChildControls();
			base.OnLoad(e);
		}*/

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

		public void Save(ContentItem item)
		{
			EnsureChildControls();
			Zeus.Context.AdminManager.Save(item, PropertyControls, Page.User);
		}

		#endregion
	}
}
