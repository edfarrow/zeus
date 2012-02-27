using Zeus.Editors.Attributes;
using Zeus.Integrity;
using Zeus.Security;

namespace Zeus.Web.Security
{
	[ContentType("Password Reset Request")]
	[RestrictParents(typeof(User))]
	public class PasswordResetRequest : DataContentItem
	{
		public PasswordResetRequest()
		{
			Title = "Password Reset Request";
		}

		[TextBoxEditor("Nonce", 100)]
		public virtual string Nonce { get; set; }

		[TextBoxEditor("Used", 110)]
		public virtual bool Used { get; set; }
	}
}