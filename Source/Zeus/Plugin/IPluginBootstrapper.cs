using System.Collections.Generic;
using Ninject;
using Zeus.Engine;

namespace Zeus.Plugin
{
	/// <summary>
	/// Finds plugins and calls their initializer.
	/// </summary>
	public interface IPluginBootstrapper
	{
		IEnumerable<IPluginDefinition> GetPluginDefinitions();
		void InitializePlugins(IKernel kernel, IEnumerable<IPluginDefinition> plugins);
	}
}