using NUnit.Framework;
using Zeus.BaseLibrary.Reflection;
using Zeus.Engine;
using Zeus.Tests.Definitions.Items;
using Zeus.ContentTypes;

namespace Zeus.Tests.Definitions
{
	[TestFixture]
	public class DefinitionTests
	{
		private ContentTypeManager _definitionManager;

		[SetUp]
		public void SetUp()
		{
			IAssemblyFinder assemblyFinder = new AssemblyFinder();
			ITypeFinder typeFinder = new TypeFinder(assemblyFinder);
			ContentTypeBuilder contentTypeBuilder = new ContentTypeBuilder(typeFinder);
			_definitionManager = new ContentTypeManager(contentTypeBuilder);
		}
	}
}
