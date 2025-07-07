using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StoreApp.Infrastructure.Services;
using Microsoft.Extensions.Options;
using StoreApp.Application.Shared.Settings;

namespace StoreApp.Persistence.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(_emailSettings.From),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mail.To.Add(to);

                using var smtp = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort)
                {
                    Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPass),
                    EnableSsl = true
                };

                await smtp.SendMailAsync(mail);
                return true;
            }
            catch (Exception ex)
            {
                // log ex
                return false;
            }
        }
    }

}
