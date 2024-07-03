using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Products.Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace Products.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Your App Name", _configuration["EmailSettings:Username"]));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };

            using (var client = new SmtpClient())
            {
                client.CheckCertificateRevocation = false;
                var host = _configuration["EmailSettings:Host"];
                var port = int.Parse(_configuration["EmailSettings:Port"]);
                var username = _configuration["EmailSettings:Username"];
                var password = _configuration["EmailSettings:Password"];

                await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(username, password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
