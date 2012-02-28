using Ninject;
using Zeus.Engine;

namespace Zeus.Plugin
{
	/// <summary>
	/// Classes implementing this interface define plugins and are responsible of
	/// calling plugin initializers.
	/// </summary>
	public interface IPluginDefinition
	{
		/// <summary>Executes the plugin initializer.</summary>
		/// <param name="kernel">A reference to the current Ninject kernel.</param>
		void Initialize(IKernel kernel);
	}
}