using Ext.Net;

namespace Zeus.Util
{
	public static class IconUtility
	{
		private static readonly ResourceManager ResourceManager = new ResourceManager();

		public static string GetIconUrl(Icon icon)
		{
			return ResourceManager.GetIconUrl(icon);
		}
	}
}
