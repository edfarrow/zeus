using System;

namespace Zeus.Web.Caching
{
	public static class CachingExtensions
	{
		private const string PageCacheEnabledKey = "PageCache_Enabled";
		private const string PageCacheDurationKey = "PageCache_Duration";

		public static bool GetPageCachingEnabled(this ContentItem contentItem)
		{
			return contentItem.IsPage && (bool) (contentItem[PageCacheEnabledKey] ?? false);
		}

		public static void SetPageCachingEnabled(this ContentItem contentItem, bool enabled)
		{
			contentItem[PageCacheEnabledKey] = enabled;
		}

		public static TimeSpan GetPageCachingDuration(this ContentItem contentItem)
		{
			return (TimeSpan) (contentItem[PageCacheDurationKey] ?? TimeSpan.FromHours(1));
		}

		public static void SetPageCachingDuration(this ContentItem contentItem, TimeSpan duration)
		{
			contentItem[PageCacheDurationKey] = duration;
		}
	}
}