using System.Collections.Generic;
using Ormongo;
using Zeus.EditableTypes;
using Zeus.Util;

namespace Zeus
{
	public class EmbeddedItem : EmbeddedDocument<ContentItem>, IEditableObject
	{
		/// <summary>
		/// Data store for arbitrary key/value pairs
		/// </summary>
		public Dictionary<string, object> ExtraData { get; set; }

		/// <summary>Used primarily by editors to provide untyped access to this item's properties. If this class
		/// does not contain the specified property, it falls back to the ExtraData dictionary.</summary>
		/// <param name="propertyName">The name of the propery or key into ExtraData.</param>
		/// <returns>The value of the property. If it could not be found, null is returned.</returns>
		public virtual object this[string propertyName]
		{
			get { return EditableObjectUtility.GetValue(this, propertyName); }
			set { EditableObjectUtility.SetValue(this, propertyName, value); }
		}

		public EmbeddedItem()
		{
			ExtraData = new Dictionary<string, object>();
		}
	}
}