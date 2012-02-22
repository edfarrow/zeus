using Ext.Net;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType]
	[RestrictParents(typeof(Shop))]
	public class Category : BasePage
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.PageGreen); }
		}

		[HtmlTextBoxEditor("Description", 200)]
		public virtual string Description { get; set; }

		public string PossessiveTitle
		{
			get { return Title + "'s"; }
		}

		public Shop Shop
		{
			get { return (Shop) Parent; }
		}
	}
}