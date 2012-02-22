using System.ComponentModel;
using Ext.Net;
using MongoDB.Bson.Serialization.Attributes;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Web;

namespace Zeus.Templates.ContentTypes
{
	/// <summary>
	/// Redirects to somewhere else. Used as a placeholder in the menu.
	/// </summary>
	[ContentType("Redirect", "Redirect", "Redirects to another page on the site.", "", 40)]
	[RestrictParents(typeof(PageContentItem))]
	public class Redirect : BasePage
	{
		public override string Url
		{
			get { return BaseLibrary.Web.Url.ToAbsolute(RedirectItem.Url); }
		}

		[LinkedItemDropDownListEditor("Redirect to", 30, Required = true, TypeFilter = typeof(PageContentItem), ContainerName = "Content")]
		public virtual ContentItem RedirectItem { get; set; }

		[CheckBoxEditor("Check Children for Navigation State", "", 40, Description = "For example, uncheck this for a 'Home' redirect item, otherwise you will have two highlighted items in the navigation.")]
		[BsonDefaultValue(true)]
		public virtual bool CheckChildrenForNavigationState { get; set; }

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.PageGo); }
		}
	}
}