using System;
using System.Collections;
using System.Security.Principal;
using System.Web;
using Zeus.BaseLibrary.Web;

namespace Zeus.Web
{
	public class WebRequestContext : IWebContext
	{
		/// <summary>Provides access to HttpContext.Current.</summary>
		protected virtual HttpContext CurrentHttpContext
		{
			get
			{
				if (System.Web.HttpContext.Current == null)
					throw new Exception("Tried to retrieve HttpContext.Current but it's null. This may happen when working outside a request or when doing stuff after the context has been recycled.");
				return System.Web.HttpContext.Current;
			}
		}

		public IHttpHandler Handler
		{
			get { return CurrentHttpContext.Handler; }
		}

		public Url LocalUrl
		{
			get { return Url.Parse(CurrentHttpContext.Request.RawUrl); }
		}

		/// <summary>Gets a dictionary of request scoped items.</summary>
		public IDictionary RequestItems
		{
			get { return CurrentHttpContext.Items; }
		}

		/// <summary>The current request object.</summary>
		public HttpRequestBase Request
		{
			get { return new HttpRequestWrapper(CurrentHttpContext.Request); }
		}

		/// <summary>The current request object.</summary>
		public HttpResponseBase Response
		{
			get { return new HttpResponseWrapper(CurrentHttpContext.Response); }
		}

		/// <summary>The current session object.</summary>
		public HttpSessionStateBase Session
		{
			get { return new HttpSessionStateWrapper(CurrentHttpContext.Session); }
		}

		public Url Url
		{
			get { return new Url(Request.Url.Scheme, Request.Url.Authority, Request.RawUrl); }
		}

		/// <summary>Gets the current user in the web execution context.</summary>
		public IPrincipal User
		{
			get { return CurrentHttpContext.User; }
		}

		public string MapPath(string path)
		{
			return CurrentHttpContext.Server.MapPath(path);
		}

		public void RewritePath(string path)
		{
			CurrentHttpContext.RewritePath(path);
		}

		public string ToAbsolute(string virtualPath)
		{
			return Url.ToAbsolute(virtualPath);
		}

		public string ToAppRelative(string virtualPath)
		{
			return VirtualPathUtility.ToAppRelative(virtualPath);
		}

		public ContentItem CurrentPage
		{
			get { return CurrentHttpContext.Items["CurrentPage"] as ContentItem; }
			set { CurrentHttpContext.Items["CurrentPage"] = value; }
		}

		/// <summary>The physical path on disk to the requested resource.</summary>
		public virtual string PhysicalPath
		{
			get { return Request.PhysicalPath; }
		}

		public HttpContextBase HttpContext
		{
			get { return new HttpContextWrapper(CurrentHttpContext); }
		}

		public string GetFullyQualifiedUrl(string url)
		{
			return Url.HostUrl + url;
		}
	}
}
