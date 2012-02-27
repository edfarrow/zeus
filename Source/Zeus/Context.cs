using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Zeus.BaseLibrary.Collections;
using Zeus.ContentTypes;
using Zeus.Engine;
using Zeus.Web;

namespace Zeus
{
	public static class Context
	{
		public static ContentEngine Current
		{
			get
			{
				if (Singleton<ContentEngine>.Instance == null)
					Initialize(false);
				return Singleton<ContentEngine>.Instance;
			}
		}

		public static ContentItem RootItem
		{
			get { return ContentItem.Find(Current.Host.CurrentSite.RootItemID); }
		}

		public static ContentItem StartPage
		{
			get { return Current.UrlParser.StartPage; }
		}

		public static Admin.IAdminManager AdminManager
		{
			get { return Current.AdminManager; }
		}

		public static IContentTypeManager ContentTypes
		{
			get { return Current.ContentTypes; }
		}

		public static IEditableTypeManager EditableTypes
		{
			get { return Current.EditableTypes; }
		}

		/// <summary>
		/// Gets the current page. This is retrieved by the page querystring.
		/// </summary>
		public static ContentItem CurrentPage
		{
			get { return Current.UrlParser.CurrentPage; }
		}

		public static Security.ISecurityManager SecurityManager
		{
			get { return Current.SecurityManager; }
		}

		public static Web.Security.IWebSecurityService WebSecurity
		{
			get { return Current.WebSecurity; }
		}

		public static IUrlParser UrlParser
		{
			get { return Current.UrlParser; }
		}

		/// <summary>Initializes a static instance of the Zeus context.</summary>
		/// <param name="forceRecreate">Creates a new context instance even though the context has been previously initialized.</param>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static ContentEngine Initialize(bool forceRecreate)
		{
			if (Singleton<ContentEngine>.Instance == null || forceRecreate)
			{
				Debug.WriteLine("Constructing engine " + DateTime.Now);
				Singleton<ContentEngine>.Instance = CreateEngineInstance();
				Debug.WriteLine("Initializing engine " + DateTime.Now);
				Singleton<ContentEngine>.Instance.Initialize();
			}
			return Singleton<ContentEngine>.Instance;
		}

		private static ContentEngine CreateEngineInstance()
		{
			return new ContentEngine(EventBroker.Instance);
		}
	}
}
