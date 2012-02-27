using System;
using System.Collections.Generic;
using System.Linq;
using Zeus.BaseLibrary.Reflection;

namespace Zeus.ContentTypes
{
	public class ContentTypeBuilder : IContentTypeBuilder
	{
		#region Fields

		private readonly ITypeFinder _typeFinder;

		#endregion

		#region Constructor

		public ContentTypeBuilder(ITypeFinder typeFinder)
		{
			_typeFinder = typeFinder;
		}

		#endregion

		#region Methods

		public IDictionary<Type, ContentType> GetContentTypes()
		{
			IList<ContentType> contentTypes = FindContentTypes();
			ExecuteRefiners(contentTypes);
			return contentTypes.ToDictionary(ct => ct.ItemType);
		}

		private IList<ContentType> FindContentTypes()
		{
			var contentTypes = EnumerateTypes().Select(type => new ContentType(type)).ToList();
			contentTypes.Sort();

			return contentTypes;
		}

		protected void ExecuteRefiners(IList<ContentType> contentTypes)
		{
			foreach (var contentType in contentTypes)
				foreach (IContentTypeRefiner refiner in contentType.ItemType.GetCustomAttributes(typeof(IContentTypeRefiner), false))
					refiner.Refine(contentType, contentTypes);
			foreach (var contentType in contentTypes)
				foreach (IInheritableContentTypeRefiner refiner in contentType.ItemType.GetCustomAttributes(typeof(IInheritableContentTypeRefiner), true))
					refiner.Refine(contentType, contentTypes);
		}

		private IEnumerable<Type> EnumerateTypes()
		{
			return _typeFinder.Find(typeof (ContentItem)).Where(t => !t.IsAbstract);
		}

		#endregion
	}
}