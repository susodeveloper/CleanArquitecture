using System.Net.Mail;
using CleanArchitecture.Application.Abstractions.Email;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    public GmailSettings _gmailSettings { get; }
     public EmailService(IOptions<GmailSettings> gmailSettings)
    {
        _gmailSettings = gmailSettings.Value;
    }

    public void Send(string receipient, string subject, string body)
    {
        try
        {
            var message = new MailMessage();
            message.From = new MailAddress(_gmailSettings.Username!);
            message.To.Add(new MailAddress(receipient));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = _gmailSettings.Port,
                Credentials = new System.Net.NetworkCredential(_gmailSettings.Username, _gmailSettings.Password),
                EnableSsl = true,
            };

            smtpClient.Send(message);

        }
        catch (Exception ex)
        {
            throw new Exception("no se pudo enviar el email", ex);
        }
    }
}