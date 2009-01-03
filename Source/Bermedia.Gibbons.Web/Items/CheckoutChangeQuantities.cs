﻿using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "[Internal Use Only]")]
	[RestrictParents(typeof(StartPage))]
	public class CheckoutChangeQuantities : StructuralPage
	{
		protected override string IconName
		{
			get { return "page"; }
		}
	}
}
