using Ext.Net;

namespace Zeus.Web.UI.WebControls
{
	public class TimeRange : CompositeField
	{
		private TimeField _from, _to;

		public string StartTitle { get; set; }
		public bool StartRequired { get; set; }
		public string BetweenText { get; set; }

		public string From
		{
			get
			{
				EnsureChildControls();
				return _from.Text;
			}
			set
			{
				EnsureChildControls();
				_from.Text = value;
			}
		}

		public string To
		{
			get
			{
				EnsureChildControls();
				return _to.Text;
			}
			set
			{
				EnsureChildControls();
				_to.Text = value;
			}
		}

		protected override void CreateChildControls()
		{
			_from = new TimeField { ID = ID + "From", FieldLabel = StartTitle, AllowBlank = !StartRequired };
			_to = new TimeField { ID = ID + "To" };

			Items.Add(_from);
			if (!string.IsNullOrEmpty(BetweenText))
				Items.Add(new DisplayField { Text = BetweenText });
			Items.Add(_to);

			base.CreateChildControls();
		}
	}
}
