using System.Collections.Generic;
using System.Linq;
using Ext.Net;
using MongoDB.Bson.Serialization.Attributes;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Linq;
using Zeus.Templates.Services.Syndication;
using Zeus.Web;

namespace Zeus.Templates.ContentTypes
{
	[ContentType("Feed", Description = "An RSS feed that outputs XML with the latest feed data.")]
	[RestrictParents(typeof(WebsiteNode), typeof(Page))]    
	public class RssFeed : PageContentItem, IFeed, INode
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.Feed); }
		}

		[LinkedItemDropDownListEditor("Feed root", 90)]
		public virtual ContentItem FeedRoot { get; set; }

		[TextBoxEditor("Number of items", 100, 10)]
		[BsonDefaultValue(10)]
		public virtual int NumberOfItems { get; set; }

		[TextBoxEditor("Tagline", 110, Required = true)]
		public virtual string Tagline { get; set; }

		[TextBoxEditor("Author", 120)]
		public virtual string Author { get; set; }

		[TextBoxEditor("RFC 3229 Delta Encoding Enabled", 130)]
		[BsonDefaultValue(true)]
		public virtual bool Rfc3229DeltaEncodingEnabled { get; set; }

		public override string Url
		{
			get { return base.Url + "?hungry=yes"; }
		}

		public virtual IEnumerable<ISyndicatable> GetItems()
		{
			return Zeus.Find.EnumerateAccessibleChildren(FeedRoot ?? Zeus.Find.StartPage)
				.OfType<ISyndicatable>()
				.OfType<ContentItem>()
				.NavigablePages()
				.Where(ci => (bool) (ci[SyndicatableDefinitionAppender.SyndicatableDetailName] ?? true))
				.OrderByDescending(ci => ci.Published)
				.Take(NumberOfItems)
				.Cast<ISyndicatable>();
		}
	}
}