using System;
using MongoDB.Bson.Serialization.Attributes;
using Zeus.Editors.Attributes;

namespace Zeus.Templates.ContentTypes
{
	[ContentType("Registration Page", "TemplatesRegistrationPage")]
	public class RegistrationPage : RegistrationPageBase
	{
		[TextBoxEditor("Verification Email Sender", 100, Description = "Email address which should be used as the sender email for the verification email which will be sent to newly registered users.")]
		public virtual string VerificationEmailSender { get; set; }

		[TextBoxEditor("Verification Email Subject", 110, Description = "Subject line of the verification email which will be sent to newly registered users.")]
		[BsonDefaultValue("Please verify your email address")]
		public virtual string VerificationEmailSubject { get; set; }

		[TextBoxEditor("Verification Email Body", 120,
		Description = "Body text of the verification email which will be sent to newly registered users. This text MUST contain the text \"{VERIFICATIONLINK}\".",
		TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine)]
		[BsonDefaultValue(@"Please click the following link to verify your email address:
{VERIFICATIONLINK}")]
		public virtual string VerificationEmailBody { get; set; }
	}
}