using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Vector.User.API.Models;
using Vector.User.API.Services.IServices;

namespace Vector.User.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void SendEmail(EmailDTO emailDTO)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(configuration.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(emailDTO.To));
            email.Subject = "Email verification with OTP";
            email.Body = new TextPart(TextFormat.Html) { Text = emailDTO.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(configuration.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(configuration.GetSection("EmailUsername").Value, configuration.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }


    }
}
