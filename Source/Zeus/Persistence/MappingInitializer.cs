using System;
using MongoDB.Bson.Serialization;
using Ninject;
using Ormongo.IdentityMap;
using Zeus.ContentTypes;

namespace Zeus.Persistence
{
	public class MappingInitializer : IInitializable
	{
		private readonly IContentTypeManager _contentTypeManager;

		public MappingInitializer(IContentTypeManager contentTypeManager)
		{
			_contentTypeManager = contentTypeManager;
		}

		public void Initialize()
		{
			ContentItem.Plugins.Add(new IdentityMap<ContentItem>());
			ContentItem.CacheDepth = true;

			foreach (var contentType in _contentTypeManager.GetContentTypes())
				BsonClassMap.RegisterClassMap(new UntypedBsonClassMap(contentType.ItemType));
		}

		private class UntypedBsonClassMap : BsonClassMap
		{
			public UntypedBsonClassMap(Type classType)
				: base(classType)
			{
				AutoMap();
			}
		}
	}
}