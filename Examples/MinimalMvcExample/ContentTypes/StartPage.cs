using MongoDB.Bson.Serialization.Attributes;
using Zeus;
using Zeus.Editors.Attributes;
using Zeus.Web;
using Zeus.Templates.ContentTypes;
using Zeus.AddIns.ECommerce.PaypalExpress.Mvc.ContentTypeInterfaces;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
    [ContentType("StartPage")]
    public class StartPage : WebsiteNode, IStartPageForPayPal
    {
        #region IStartPageForPayPal Members

		[TextBoxEditor("Paypal API Username", 100)]
		public virtual string APIUsername { get; set; }

		[TextBoxEditor("Paypal API Password", 110)]
		public virtual string APIPassword { get; set; }

		[TextBoxEditor("Paypal API Signature", 120)]
		public virtual string APISignature { get; set; }

		[TextBoxEditor("Paypal Test API Username", 130)]
		public virtual string TestAPIUsername { get; set; }

		[TextBoxEditor("Paypal Test API Password", 140)]
		public virtual string TestAPIPassword { get; set; }

		[TextBoxEditor("Paypal Test API Signature", 150)]
		public virtual string TestAPISignature { get; set; }

		[TextBoxEditor("Use Paypal Test Environment", 160, Description = "Make sure this is NOT checked if you want the site to take real payments")]
		[BsonDefaultValue(true)]
		public bool UseTestEnvironment { get; set; }

        #endregion
    }
}
