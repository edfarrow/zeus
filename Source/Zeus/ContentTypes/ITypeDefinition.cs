using System;
using System.Collections.Generic;
using System.Security.Principal;
using Zeus.Editors.Attributes;

namespace Zeus.ContentTypes
{
	public interface ITypeDefinition
	{
		Type ItemType { get; }
		

		string Title { get; }
	}
}