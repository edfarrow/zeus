using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Ninject;
using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Planning.Bindings;

namespace Zeus.Engine
{
	public class InitializableKernel : StandardKernel
	{
		private readonly List<IBinding> _bindings;

		public InitializableKernel()
		{
			_bindings = new List<IBinding>();
		}

		public override void AddBinding(IBinding binding)
		{
			_bindings.Add(binding);
			base.AddBinding(binding);
		}

		public void InitializeServices()
		{
			Type initializableInterfaceType = typeof(IInitializable);
			Type startableInterfaceType = typeof(IStartable);
			foreach (IBinding binding in _bindings)
			{
				IContext context = CreateContext(CreateRequest(binding.Service, null, new List<IParameter>(), false, false), binding);
				if (binding.GetProvider(context).Type.GetInterfaces().Any(i => i == initializableInterfaceType || i == startableInterfaceType))
					this.GetAll(binding.Service).ToList(); // Force creation.
			}
		}
	}

	public class ZeusKernel : InitializableKernel
	{
		public ZeusKernel()
		{
			// Get all DLLS in bin folder.
			// TODO: Remove this hack.
			string directory = (HttpContext.Current != null)
				? Path.GetDirectoryName(HttpRuntime.BinDirectory)
				: AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
			IEnumerable<string> files = Directory.GetFiles(directory, "*.dll");

			//// Load modules in Zeus DLLs first.
			Load(FindAssemblies(files.Where(s => Path.GetFileName(s).StartsWith("Zeus."))));

			//// Then load non-Zeus DLLs - this gives projects a chance to override Zeus modules.
			//// Actually we just load all DLLs, because DLLs that have already been loaded
			//// won't get loaded again.
			Load(FindAssemblies(files.Where(s => !Path.GetFileName(s).StartsWith("Zeus."))));
		}

		private static IEnumerable<Assembly> FindAssemblies(IEnumerable<string> filenames)
		{
			return FindAssemblyNames(filenames, a => true).Select(Assembly.Load);
		}

		private static IEnumerable<AssemblyName> FindAssemblyNames(IEnumerable<string> filenames, Predicate<Assembly> filter)
		{
			AppDomain temporaryDomain = CreateTemporaryAppDomain();

			foreach (string file in filenames)
			{
				Assembly assembly;

				if (File.Exists(file))
				{
					try
					{
						var name = new AssemblyName { CodeBase = file };
						assembly = temporaryDomain.Load(name);
					}
					catch (BadImageFormatException)
					{
						// Ignore native assemblies
						continue;
					}
				}
				else
				{
					assembly = temporaryDomain.Load(file);
				}

				if (filter(assembly))
				{
					yield return assembly.GetName();
				}
			}

			AppDomain.Unload(temporaryDomain);
		}

		private static AppDomain CreateTemporaryAppDomain()
		{
			return AppDomain.CreateDomain(
				"AssemblyScanner",
				AppDomain.CurrentDomain.Evidence,
				AppDomain.CurrentDomain.SetupInformation);
		}
	}
}