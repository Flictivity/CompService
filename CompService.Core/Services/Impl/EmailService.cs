using CompService.Core.Results;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace CompService.Core.Services.Impl;

public class EmailService : IEmailService
{
    private readonly ILogger<IEmailService> _logger;

    public EmailService(ILogger<IEmailService> logger)
    {
        _logger = logger;
    }

    public async Task<BaseResult> SendEmailAsync(string? email, string subject, string message)
    {
        var sendMessage = new MimeMessage();
        sendMessage.From.Add(new MailboxAddress("Компьютерный сервис", "computerservlse@mail.ru"));
        sendMessage.To.Add(MailboxAddress.Parse(email));
        sendMessage.Subject = subject;
        sendMessage.Body = new TextPart("plain")
        {
            Text = message
        };

        SmtpClient client = new SmtpClient();

        try
        {
            client.Connect("smpt.mail.ru", 465, true);
            client.Authenticate("computerservlse@mail.ru","huHgtZq7qVDhrfUYk8hu");
            await client.SendAsync(sendMessage);
            _logger.LogInformation("Письмо успешно отправлено");
            return new BaseResult{Success = true};
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new BaseResult{Success = false, Message = "Не удалось отправить письмо"};
        }
        finally
        {
            client.Disconnect(true);
            client.Dispose();
        }
    }
}