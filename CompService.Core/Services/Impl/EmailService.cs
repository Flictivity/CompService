﻿using MailKit.Net.Smtp;
using MailKit.Security;
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

    public async Task SendEmailAsync(string email, string subject, string message)
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
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        finally
        {
            client.Disconnect(true);
            client.Dispose();
        }
    }
}