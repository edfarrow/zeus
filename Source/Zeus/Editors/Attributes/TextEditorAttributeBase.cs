using System;
using System.Web.UI;
using Zeus.EditableTypes;

namespace Zeus.Editors.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public abstract class TextEditorAttributeBase : AbstractEditorAttribute
	{
		protected TextEditorAttributeBase()
		{
			
		}

		/// <summary>Initializes a new instance of the EditableTextBoxAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		protected TextEditorAttributeBase(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		/// <summary>Initializes a new instance of the EditableTextBoxAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		/// <param name="maxLength">The max length of the text box.</param>
		protected TextEditorAttributeBase(string title, int sortOrder, int maxLength)
			: this(title, sortOrder)
		{
			MaxLength = maxLength;
		}

		#region Properties

		/// <summary>Gets or sets the max length of the text box.</summary>
		public int MaxLength { get; set; }

		/// <summary>Gets or sets the default value. When the editor's value equals this value then null is saved instead.</summary>
		public string DefaultValue { get; set; }

		public bool ReadOnly { get; set; }

		#endregion

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			ITextControl tb = editor as ITextControl;
			string value = (tb.Text == DefaultValue) ? null : tb.Text;
            if (!AreEqual(value, (item[Name] == null ? null : item[Name].ToString())))
			{
				item[Name] = value;
				return true;
			}
			return false;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			ITextControl tb = editor as ITextControl;
			tb.Text = ConvertUtility.Convert<string>(item[Name]) ?? DefaultValue;
		}
	}
}