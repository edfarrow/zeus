using System.Collections.Generic;

namespace Zeus.ContentTypes
{
	public interface IContentTypeRefiner
	{
		void Refine(ContentType currentContentType, IList<ContentType> allContentTypes);
	}
}