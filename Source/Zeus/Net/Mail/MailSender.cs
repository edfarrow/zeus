using System.Net.Mail;

namespace Zeus.Net.Mail
{
	/// <summary>
	/// Sends the mail message through the smtp server configured in <system.net>.
	/// </summary>
	public class MailSender : SmtpMailSender
	{
		protected override SmtpClient GetSmtpClient()
		{
            if (System.Configuration.ConfigurationManager.AppSettings["MailServer"] != null)
				return CreateSmtpClient(System.Configuration.ConfigurationManager.AppSettings["MailServer"], -1, null, null);
            else
                return CreateSmtpClient(null, -1, null, null);
		}
	}
}