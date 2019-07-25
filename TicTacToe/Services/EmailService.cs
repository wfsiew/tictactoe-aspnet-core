using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TicTacToe.Options;

namespace TicTacToe.Services
{
    public class EmailService : IEmailService
    {
        private EmailServiceOptions m_emailServiceOptions;

        public EmailService(IOptions<EmailServiceOptions> emailServiceOptions)
        {
            m_emailServiceOptions = emailServiceOptions.Value;
        }

        public Task SendEmail(string emailTo, string subject, string message)
        {
            using (var client = new SmtpClient(m_emailServiceOptions.MailServer,
                int.Parse(m_emailServiceOptions.MailPort)))
            {
                if (bool.Parse(m_emailServiceOptions.UseSSL) == true)
                    client.EnableSsl = true;

                if (!string.IsNullOrEmpty(m_emailServiceOptions.UserId))
                    client.Credentials = new NetworkCredential(m_emailServiceOptions.UserId,
                    m_emailServiceOptions.Password);

                client.Send(new MailMessage("example@example.com", emailTo, subject, message));
            }

            return Task.CompletedTask;
        }
    }
}
