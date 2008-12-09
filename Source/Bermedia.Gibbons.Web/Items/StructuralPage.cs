﻿using System;
using Zeus.ContentTypes.Properties;
using Zeus;

namespace Bermedia.Gibbons.Items
{
	public abstract class StructuralPage : ContentItem
	{
		[EditableTextBox("Title", 10)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[NameEditor("Name", 20)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		public override string IconUrl
		{
			get { return "~/Gibbons/Assets/Images/Icons/" + this.IconName + ".png"; }
		}

		protected abstract string IconName { get; }

		public override string TemplateUrl
		{
			get { return "~/Gibbons/UI/Views/" + this.TemplateName + ".aspx"; }
		}

		protected virtual string TemplateName
		{
			get { return this.GetType().Name; }
		}
	}
}
