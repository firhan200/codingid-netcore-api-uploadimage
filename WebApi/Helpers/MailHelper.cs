using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace WebApi.Helpers
{
    public static class MailHelper
    {
        public static async Task Send(string toEmail, string subject, string messageText) {
            /* configuration */
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("No Reply", "noreply@coding.id"));
            message.To.Add(new MailboxAddress("Firhan", toEmail));
            message.Subject = subject;
            /* configuration */

            /* masukin message text */
            message.Body = new TextPart("plain")
            {
                Text = messageText
            };
            /* masukin message text */

            /* kirim email nya */
            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("testingcodingidemail@gmail.com", "kfoqxdznomzkugiv");
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
            }
            /* kirim email nya */

        }
    }
}
