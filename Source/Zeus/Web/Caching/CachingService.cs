﻿using System;
using System.Web.Caching;
using Ninject;
using Ormongo;
using Zeus.Persistence;

namespace Zeus.Web.Caching
{
	public class CachingService : ICachingService, IStartable
	{
		private readonly IWebContext _webContext;

		public CachingService(IWebContext webContext)
		{
			_webContext = webContext;
		}

		public bool IsPageCached(ContentItem contentItem)
		{
			return _webContext.HttpContext.Cache[GetCacheKey(contentItem)] != null;
		}

		public void InsertCachedPage(ContentItem contentItem, string html)
		{
			_webContext.HttpContext.Cache.Insert(GetCacheKey(contentItem),
				html, null, DateTime.Now.Add(contentItem.GetPageCachingDuration()),
				Cache.NoSlidingExpiration);
		}

		public string GetCachedPage(ContentItem contentItem)
		{
			object cachedPage = _webContext.HttpContext.Cache[GetCacheKey(contentItem)];
			if (cachedPage == null)
				throw new InvalidOperationException("Page is not cached.");

			return (string) cachedPage;
		}

		public void DeleteCachedPage(ContentItem contentItem)
		{
			_webContext.HttpContext.Cache.Remove(GetCacheKey(contentItem));
		}

		private string GetCacheKey(ContentItem contentItem)
		{
			//return "ZeusPageCache_" + contentItem.ID;
			//changed to make sure that the querystring is considered
			if (_webContext.HttpContext.Request.QueryString == null)
				return "ZeusPageCache_" + contentItem.ID;
			else
				return "ZeusPageCache_" + contentItem.ID + "_" + _webContext.HttpContext.Request.QueryString;
		}

		#region IStartable methods

		public void Start()
		{
			ContentItem.BeforeSave += OnPersisterItemSaving;
		}

		private void OnPersisterItemSaving(object sender, CancelDocumentEventArgs<ContentItem> e)
		{
			if (IsPageCached(e.Document))
				DeleteCachedPage(e.Document);
		}

		public void Stop()
		{
			ContentItem.BeforeSave -= OnPersisterItemSaving;
		}

		#endregion
	}
}