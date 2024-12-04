using Core.Interfaces.IServices.Common;
using MailKit.Net.Smtp;
using MimeKit;

namespace Core.Services.Common
{
    public class EmailService : IEmailService
    {
        private const string smtp_host = "smtp.gmail.com";
        private const int smtp_port = 587;
        private const string SYSTEM_EMAIL = "SYSTEM_EMAIL";
        private const string SYSTEM_EMAIL_PASS = "SYSTEM_EMAIL_PASS";
        public async Task<bool> SendTo(string receiver, string subject, string message)
        {
            try
            {
                string email = Environment.GetEnvironmentVariable(SYSTEM_EMAIL);
                string password = Environment.GetEnvironmentVariable(SYSTEM_EMAIL_PASS);

                var mail = new MimeMessage();
                mail.From.Add(new MailboxAddress("ZLEARN", email));
                mail.To.Add(MailboxAddress.Parse(receiver));
                mail.Subject = subject;
                mail.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message
                };

                using (var emailClient = new SmtpClient())
                {
                    emailClient.Connect(smtp_host, smtp_port, MailKit.Security.SecureSocketOptions.StartTls);
                    emailClient.Authenticate(email, password);
                    await emailClient.SendAsync(mail);
                    emailClient.Disconnect(true);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
