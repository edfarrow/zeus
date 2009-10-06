using System;
using Coolite.Ext.Web;
using Zeus.Configuration;
using Zeus.Security;

namespace Zeus.Admin.ActionPlugins
{
	public abstract class LanguageActionPluginBase : ActionPluginBase
	{
		public override string GroupName
		{
			get { return "Globalization"; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.Administer; }
		}

		public override bool IsApplicable(ContentItem contentItem)
		{
			// Hide, if globalization is disabled.
			if (!Context.Current.Resolve<GlobalizationSection>().Enabled)
				return false;

			// Hide, if current item is not translatable.
			if (!Context.Current.LanguageManager.CanBeTranslated(contentItem))
				return false;

			return base.IsApplicable(contentItem);
		}
	}
}