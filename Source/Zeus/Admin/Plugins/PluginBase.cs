using System;

namespace Zeus.Admin.Plugins
{
	public abstract class PluginBase
	{
		protected string GetPageUrl(Type type, string resourcePath)
		{
			return Context.Current.GetServerResourceUrl(type.Assembly, resourcePath);
		}

		public static string GetAdminDefaultUrl()
		{
			// TODO: Ideally we can make Ext.NET direct methods not need this URL.
			return "/" + Context.AdminManager.AdminPath + "/default.aspx";
		}
	}
}