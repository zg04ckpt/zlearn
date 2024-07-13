using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Application.Common
{
    public class EmailSender : IEmailSender
    {

        private const string smtp_host = "smtp.gmail.com";
        private const int smtp_port = 587;
        public async Task<bool> SendTo(string receiver, string subject, string message)
        {
            try
            {
                string email = Environment.GetEnvironmentVariable(Consts.EnvKey.SYSTEM_EMAIL);
                string password = Environment.GetEnvironmentVariable(Consts.EnvKey.SYSTEM_EMAIL_PASS);

                var mail = new MimeMessage();
                mail.From.Add(new MailboxAddress("CodeAndLife", email));
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
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
