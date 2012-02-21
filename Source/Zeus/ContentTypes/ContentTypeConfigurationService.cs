using Ninject;
using Zeus.Configuration;

namespace Zeus.ContentTypes
{
	public class ContentTypeConfigurationService : IInitializable
	{
		#region Fields

		private readonly ContentTypesSection _configSection;
		private readonly IContentTypeManager _contentTypeManager;

		#endregion

		#region Constructor

		public ContentTypeConfigurationService(ContentTypesSection configSection, IContentTypeManager contentTypeManager)
		{
			_configSection = configSection;
			_contentTypeManager = contentTypeManager;
		}

		#endregion

		#region Methods

		public void Initialize()
		{
			// For each registered content types, apply filters specified in <contentTypes> config section.
			foreach (ContentType contentType in _contentTypeManager.GetContentTypes())
				if (!_configSection.Rules.IsContentTypeAllowed(contentType))
					contentType.Enabled = false;
		}

		#endregion
	}
}