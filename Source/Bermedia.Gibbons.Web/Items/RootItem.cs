﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Root Item")]
	[RestrictParents(AllowedTypes.None)]
	public class RootItem : BaseContentItem
	{
		protected override string IconName
		{
			get { return "page_gear"; }
		}

		public override bool IsPage
		{
			get { return true; }
		}
	}
}
