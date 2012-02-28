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

		protected override Icon Icon
		{
			get { return Icon.BookOpen; }
		}

		protected override void OnAfterCreate()
		{
			Children.Create(new CountryList());
			base.OnAfterCreate();
		}
	}
}
