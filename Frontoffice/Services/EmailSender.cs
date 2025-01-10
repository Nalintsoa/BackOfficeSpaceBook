using System.Net.Mail;
using System.Net;
using Frontoffice.Models;
using Microsoft.Extensions.Options;

namespace Frontoffice.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string body, byte[] attachment, string attachmentName);
    }
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, byte[] attachment, string attachmentName)
        {
            using (var client = new SmtpClient(_emailSettings.SMTPServer, _emailSettings.SMTPPort))
            {
                client.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                if (attachment != null && attachment.Length > 0)
                {
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(attachment), attachmentName));
                }

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
