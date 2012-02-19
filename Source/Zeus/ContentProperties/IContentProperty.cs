using System;
using Zeus.ContentTypes;
using Zeus.Design.Displayers;
using Zeus.Design.Editors;

namespace Zeus.ContentProperties
{
	public interface IContentProperty : IUniquelyNamed
	{
		int SortOrder { get; set; }
		string Title { get; set; }

		IDisplayer GetDefaultDisplayer();
		IEditor GetDefaultEditor();

		PropertyData CreatePropertyData(ContentItem enclosingItem, object value);
		Type GetPropertyDataType();
	}
}