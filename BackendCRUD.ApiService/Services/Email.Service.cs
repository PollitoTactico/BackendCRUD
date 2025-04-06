using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace BackendCRUD.ApiService.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendVerificationEmail(string toEmail, string token)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:Email"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "Verifica tu cuenta";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"<h1>Bienvenido!</h1><p>Por favor verifica tu cuenta usando este token: {token}</p>"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration["EmailSettings:Host"],
                int.Parse(_configuration["EmailSettings:Port"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["EmailSettings:Email"],
                _configuration["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}