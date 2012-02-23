using Ext.Net;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes.ReferenceData
{
	[ContentType("Reference Data", Description = "Container for all types of reference data, such as a list of countries.")]
	[RestrictParents(typeof(SystemNode))]
	public class ReferenceDataNode : BaseContentItem
	{
		public ReferenceDataNode()
		{
			Name = "reference-data";
			Title = "Reference Data";
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.BookOpen); }
		}

		protected override void OnAfterCreate()
		{
			Children.Create(new CountryList());
			base.OnAfterCreate();
		}
	}
}
