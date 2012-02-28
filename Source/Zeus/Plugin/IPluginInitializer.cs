using Ninject;

namespace Zeus.Plugin
{
	/// <summary>Classes implementing this interface can serve as plug in initializers.
	/// If one of these classes is referenced by a PlugInAttribute it's initialize methods 
	/// will be invoked by the Zeus engine during initialization.</summary>
	public interface IPluginInitializer
	{
		/// <summary>Invoked after the engine has been initialized.</summary>
		/// <param name="kernel">The Ninject kernel that has been initialized.</param>
		void Initialize(IKernel kernel);
	}
}