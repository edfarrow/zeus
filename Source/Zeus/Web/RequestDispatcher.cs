using System;
using System.Collections.Specialized;
using System.Web;
using Zeus.BaseLibrary.Web;
using Zeus.Configuration;

namespace Zeus.Web
{
	/// <summary>
	/// Resolves the controller to handle a certain request. Supports a default 
	/// controller or additional imprativly using the ConnectControllers method 
	/// or declarativly using the [Controls] attribute registered.
	/// </summary>
	public class RequestDispatcher : IRequestDispatcher
	{
		private readonly IWebContext _webContext;
		private readonly IUrlParser _parser;
		private readonly bool _rewriteEmptyExtension = true;
		private readonly bool observeAllExtensions = true;
		private readonly string[] _observedExtensions = new[] { ".aspx" };
		private readonly string[] _nonRewritablePaths = new[] { "~/admin/" };

		public RequestDispatcher(IWebContext webContext, IUrlParser parser, HostSection config)
		{
			this._webContext = webContext;
			this._parser = parser;
			//observeAllExtensions = config.Web.ObserveAllExtensions;
			_rewriteEmptyExtension = config.Web.ObserveEmptyExtension;
			StringCollection additionalExtensions = config.Web.ObservedExtensions;
			if (additionalExtensions != null && additionalExtensions.Count > 0)
			{
				_observedExtensions = new string[additionalExtensions.Count + 1];
				additionalExtensions.CopyTo(_observedExtensions, 1);
			}
			_observedExtensions[0] = config.Web.Extension;
			//nonRewritablePaths = config.Web.Urls.NonRewritable.GetPaths(webContext);
		}

		/// <summary>Resolves the controller for the current Url.</summary>
		/// <returns>A suitable controller for the given Url.</returns>
		public PathData ResolvePath() 
		{
			Url url = _webContext.Url;
			string path = url.Path;
			foreach (string nonRewritablePath in _nonRewritablePaths)
			{
				if (path.StartsWith(VirtualPathUtility.ToAbsolute(nonRewritablePath)))
					return null;
			}

			return ResolveUrl(url);
		}

		public PathData ResolveUrl(string url)
		{
			try
			{
				if (IsObservable(url)) return _parser.ResolvePath(url);
			}
			catch (Exception)
			{
				
			}
			return PathData.Empty;
		}

		private bool IsObservable(Url url)
		{
			if (observeAllExtensions)
				return true;

			if (url.LocalUrl == Url.ApplicationPath)
				return true;

			string extension = url.Extension;
			if (_rewriteEmptyExtension && string.IsNullOrEmpty(extension))
				return true;
			foreach (string observed in _observedExtensions)
				if (string.Equals(observed, extension, StringComparison.InvariantCultureIgnoreCase))
					return true;
			if (url.GetQuery(PathData.PageQueryKey) != null)
				return true;

			return false;
		}
	}
}