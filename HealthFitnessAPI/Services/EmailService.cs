using System.Net;
using System.Net.Mail;

namespace HealthFitnessAPI.Services;

public interface IEmailService
{
    public Task SendEmailAsync(string toAddress, string subject, string message);
}

public class EmailService(IConfiguration config) : IEmailService
{
    private readonly SmtpClient _smtpClient = new(config["SmtpConfig:Url"], config.GetValue<int>("SmtpConfig:Port"))
    {
        Credentials = new NetworkCredential(config["SmtpConfig:Username"], config["SmtpConfig:Password"]),
        EnableSsl = config.GetValue<bool>("SmtpConfig:UseSSL"),
    };

    public async Task SendEmailAsync(string toAddress, string subject, string message)
    {
        var mailMessage = new MailMessage()
        {
            From = new MailAddress(config["SmtpConfig:Username"]!),
            Subject = subject,
            Body = message
        };
        mailMessage.To.Add(new MailAddress(toAddress));
        
        await _smtpClient.SendMailAsync(mailMessage);
    }
}