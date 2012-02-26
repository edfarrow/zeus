using System.Reflection;
using System.Web.Hosting;
using Camino;

namespace Zeus.Web.Hosting
{
	/// <summary>
	/// Manages embedded *.aspx and *.ascx files, such as those used
	/// in the admin site.
	/// </summary>
	public class EmbeddedResourceManager : IEmbeddedResourceManager
	{
		private readonly EmbeddedResourcePathProvider _pathProvider;

		public VirtualPathProvider VirtualPathProvider
		{
			get { return _pathProvider; }
		}

		public EmbeddedResourceManager()
		{
			_pathProvider = new EmbeddedResourcePathProvider();
		}

		public void AddAssembly(Assembly assembly, string prefix)
		{
			_pathProvider.AddAssembly(assembly, prefix);
		}

		public string GetUrl(Assembly resourceAssembly, string resourcePath)
		{
			return _pathProvider.GetUrl(resourceAssembly, resourcePath);
		}
	}
}