﻿using System;
using System.Web.UI;
using System.Collections;
using System.ComponentModel.Design;
using System.Web.Compilation;

namespace Isis.Web.UI.WebControls
{
	/// <summary>
	/// http://www.codeproject.com/KB/aspnet/TypedRepeater.aspx
	/// </summary>
	public class TypedRepeaterControlBuilder : ControlBuilder
	{
		public override void Init(TemplateParser parser, ControlBuilder parentBuilder, Type type, string tagName, string id, IDictionary attribs)
		{
			string dataItemTypeName = attribs["DataItemTypeName"] as string;

			Type dataItemType;
			if (this.InDesigner)
			{
				ITypeResolutionService typeResolutionService = (ITypeResolutionService) this.ServiceProvider.GetService(typeof(ITypeResolutionService));
				dataItemType = typeResolutionService.GetType(dataItemTypeName);
			}
			else
			{
				dataItemType = BuildManager.GetType(dataItemTypeName, true);
			}

			Type repeaterFakeType = new TypedRepeaterFakeType(dataItemType);

			base.Init(parser, parentBuilder, repeaterFakeType, tagName, id, attribs);
		}
	}
}
