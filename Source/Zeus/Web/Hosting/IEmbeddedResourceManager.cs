using System.Reflection;
using System.Web.Hosting;

namespace Zeus.Web.Hosting
{
	public interface IEmbeddedResourceManager
	{
		VirtualPathProvider VirtualPathProvider { get; }
		void AddAssembly(Assembly assembly, string prefix);
		string GetUrl(Assembly resourceAssembly, string relativePath);
	}
}