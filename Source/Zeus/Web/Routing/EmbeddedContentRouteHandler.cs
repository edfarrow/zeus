using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using Zeus.BaseLibrary.Web;

namespace Zeus.Web.Routing
{
	public class EmbeddedContentRouteHandler : IRouteHandler
	{
		private readonly Assembly _assembly;
		private readonly string _resourcePath;

		public EmbeddedContentRouteHandler(Assembly assembly, string resourcePath)
		{
			_assembly = assembly;
			_resourcePath = resourcePath;
		}

		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			return new EmbeddedContentHttpHandler(this, requestContext);
		}

		public Stream GetStream(string resource)
		{
			return _assembly.GetManifestResourceStream(_resourcePath + "." + resource.Replace('/', '.'));
		}

		internal class EmbeddedContentHttpHandler : IHttpHandler
		{
			private readonly EmbeddedContentRouteHandler _routeHandler;
			private readonly RequestContext _requestContext;

			public EmbeddedContentHttpHandler(EmbeddedContentRouteHandler routeHandler, RequestContext requestContext)
			{
				_routeHandler = routeHandler;
				_requestContext = requestContext;
			}

			public bool IsReusable
			{
				get { return false; }
			}

			private static readonly IDictionary<Assembly, DateTime> AssemblyLastModifiedCache =
				new Dictionary<Assembly, DateTime>();

			private static DateTime GetAssemblyLastModified(Assembly assembly)
			{
				DateTime lastModified;
				if (!AssemblyLastModifiedCache.TryGetValue(assembly, out lastModified))
				{
					AssemblyName x = assembly.GetName();
					lastModified = new DateTime(File.GetLastWriteTime(new Uri(x.CodeBase).LocalPath).Ticks);
					AssemblyLastModifiedCache.Add(assembly, lastModified);
				}
				return lastModified;
			}

			public void ProcessRequest(HttpContext context)
			{
				context.Response.Clear();

				HttpCachePolicy cache = context.Response.Cache;
				cache.SetCacheability(HttpCacheability.Public);
				cache.SetOmitVaryStar(true);
				cache.SetExpires(DateTime.Now + TimeSpan.FromDays(365.0));
				cache.SetValidUntilExpires(true);
				cache.SetLastModified(GetAssemblyLastModified(_routeHandler._assembly));

				var resource = _requestContext.RouteData.GetRequiredString("resource");
				context.Response.ContentType = MimeUtility.GetMimeType(Path.GetExtension(resource));
				
				using (var stream = _routeHandler.GetStream(resource))
				{
					if (stream == null)
						throw new HttpException(404, "Could not find embedded resource with name '" + resource + "'.");

					byte[] buffer = new byte[1024];
					Stream outputStream = context.Response.OutputStream;
					int count = 1;
					while (count > 0)
					{
						count = stream.Read(buffer, 0, 1024);
						outputStream.Write(buffer, 0, count);
					}
					outputStream.Flush(); 
				}
			}
		}
	}
}