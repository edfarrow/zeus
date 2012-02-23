using System.Text.RegularExpressions;
using MongoDB.Bson;

namespace Zeus.Web
{
	public class PermanentLinkManager : IPermanentLinkManager
	{
		public string ResolvePermanentLinks(string value)
		{
			const string pattern = @"href=""/?~/link/([\d]+?)""";
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
			return regex.Replace(value, OnPatternMatched);
		}

		private string OnPatternMatched(Match match)
		{
			// Get ContentID from link.
			ObjectId contentID = ObjectId.Parse(match.Groups[1].Value);

			// Load content item and get URL.
			ContentItem contentItem = ContentItem.FindOneByID(contentID);
			return string.Format(@"href=""{0}""", (contentItem != null) ? contentItem.Url : "#");
		}
	}
}