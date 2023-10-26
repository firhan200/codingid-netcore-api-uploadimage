using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace WebApi.Helpers
{
    public static class MailHelper
    {
        public static async Task Send(string toEmail, string subject, string messageText) {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("No Reply", "noreply@coding.id"));
            message.To.Add(new MailboxAddress("Firhan", toEmail));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = messageText
            };

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("noreplytestingcodingid@gmail.com", "cplrzezpyppspvld");
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
            }

        }
    }
}
