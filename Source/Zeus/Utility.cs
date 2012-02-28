using Ext.Net;
using Zeus.BaseLibrary.ExtensionMethods;

namespace Zeus
{
	/// <summary>
	/// Mixed utility functions used by Zeus.
	/// </summary>
	public static class Utility
	{
		private static readonly ResourceManager _resourceManager = new ResourceManager();
		public static string GetCooliteIconUrl(Icon icon)
		{
			return _resourceManager.GetIconUrl(icon);
		}
	}
}
