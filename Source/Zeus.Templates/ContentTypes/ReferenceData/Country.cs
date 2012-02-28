using Ext.Net;
using MongoDB.Bson.Serialization.Attributes;
using Zeus.BaseLibrary;
using Zeus.Editors.Attributes;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes.ReferenceData
{
	[ContentType("Country")]
	[RestrictParents(typeof(CountryList))]
	public class Country : BaseContentItem
	{
		public Country()
		{

		}

		public Country(string alpha2, string ignore, string title, string alpha3, string numeric)
		{
			this.Alpha2 = alpha2;
			this.Title = title;
			this.Alpha3 = alpha3;
			this.Numeric = numeric;

			string tempIconName = "Flag" + alpha2.Substring(0, 1) + alpha2.Substring(1).ToLower();
			Icon icon;
			if (EnumHelper.TryParse(tempIconName, out icon))
				FlagIcon = icon;
		}

		[TextBoxEditor("Name", 10, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[TextBoxEditor("CountryCode", 20, Required = false)]
		public string CountryCode { get; set; }

		public string Numeric { get; set; }
		public string Alpha2 { get; set; }
		public string Alpha3 { get; set; }

		[BsonDefaultValue(Icon.Map)]
		public Icon FlagIcon { get; set; }

		protected override Icon Icon
		{
			get { return FlagIcon; }
		}
	}
}
