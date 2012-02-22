using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Web;
using Zeus.Web.Security;

namespace Zeus.Templates.ContentTypes
{
	[ContentType("Forgotten Password Page")]
	[RestrictParents(typeof(ILoginContext))]
	public class ForgottenPasswordPage : BasePage
	{
		[TextBoxEditor("Password Reset Email Sender", 100, Description = "Email address which should be used as the sender email for the email which will be sent to users who want to reset their password.")]
		public virtual string PasswordResetEmailSender { get; set; }

		[TextBoxEditor("Password Reset Email Subject", 110, Description = "Subject line of the email which will be sent to users who want to reset their password.")]
		[BsonDefaultValue("Password Reset Request")]
		public virtual string PasswordResetEmailSubject { get; set; }

		[TextBoxEditor("Password Reset Email Body", 120,
		Description = "Body text of the email which will be sent to users who want to reset their password. This text MUST contain the text \"" + CredentialService.PasswordResetLinkName + "\".",
		TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine)]
		[BsonDefaultValue(@"To initiate the password reset process for your
account, click the link below:

" + CredentialService.PasswordResetLinkName + @"

If clicking the link above doesn't work, please copy and paste the URL in a
new browser window instead.

If you've received this mail in error, it's likely that another user entered
your email address by mistake while trying to reset a password. If you didn't
initiate the request, you don't need to take any further action and can safely
disregard this email.")]
		public virtual string PasswordResetEmailBody { get; set; }
	}
}