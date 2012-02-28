using Ninject;
using Zeus.ContentTypes;
using Zeus.Editors.Attributes;

namespace Zeus.Templates.Services.Syndication
{
	/// <summary>
	/// Examines existing item definitions and add an editable checkbox detail 
	/// to the items implementing the <see cref="ISyndicatable" />
	/// interface.
	/// </summary>
	public class SyndicatableDefinitionAppender : IInitializable
	{
		private readonly IEditableTypeManager _editableTypeManager;
		private string checkBoxText = "Make available for syndication.";
		private string containerName = "Syndication";
		private int sortOrder = 30;
		public static readonly string SyndicatableDetailName = "Syndicate";

		public SyndicatableDefinitionAppender(IEditableTypeManager editableTypeManager)
		{
			_editableTypeManager = editableTypeManager;
		}

		public int SortOrder
		{
			get { return sortOrder; }
			set { sortOrder = value; }
		}

		public string ContainerName
		{
			get { return containerName; }
			set { containerName = value; }
		}

		public string CheckBoxText
		{
			get { return checkBoxText; }
			set { checkBoxText = value; }
		}

		public void Initialize()
		{
			foreach (var editableType in _editableTypeManager.GetEditableTypes())
				if (typeof(ISyndicatable).IsAssignableFrom(editableType.ItemType))
				{
					FieldSetAttribute seoTab = new FieldSetAttribute("Syndication", "Syndication", 30);
					editableType.Add(seoTab);

					CheckBoxEditorAttribute ecb = new CheckBoxEditorAttribute(CheckBoxText, string.Empty, 10)
					{
						Name = SyndicatableDetailName,
						ContainerName = ContainerName,
						SortOrder = SortOrder,
						DefaultValue = true
					};

					editableType.Add(ecb);
				}
		}
	}
}