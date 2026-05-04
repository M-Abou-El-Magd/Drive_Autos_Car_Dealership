using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Car_Dealership_System.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpHost = _configuration["Smtp:Host"];
            var smtpPort = int.TryParse(_configuration["Smtp:Port"], out var port) ? port : 25;
            var smtpUser = _configuration["Smtp:Username"];
            var smtpPassword = _configuration["Smtp:Password"];
            var fromAddress = _configuration["Smtp:From"] ?? "no-reply@cardealership.local";

            if (string.IsNullOrWhiteSpace(smtpHost))
            {
                var logDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email-logs");
                Directory.CreateDirectory(logDir);
                var logFile = Path.Combine(logDir, $"reservation-{Guid.NewGuid()}.txt");
                var logContent = $"To: {to}\nSubject: {subject}\n\n{body}";
                await File.WriteAllTextAsync(logFile, logContent);
                Console.WriteLine("[EmailService] SMTP not configured. Reservation email logged to: " + logFile);
                return;
            }

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = bool.TryParse(_configuration["Smtp:EnableSsl"], out var enableSsl) && enableSsl,
            };

            if (!string.IsNullOrWhiteSpace(smtpUser) && !string.IsNullOrWhiteSpace(smtpPassword))
            {
                client.Credentials = new NetworkCredential(smtpUser, smtpPassword);
            }

            using var email = new MailMessage(fromAddress, to, subject, body)
            {
                IsBodyHtml = false
            };

            await client.SendMailAsync(email);
        }
    }
}
