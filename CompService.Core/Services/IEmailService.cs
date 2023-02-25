namespace CompService.Core.Services;

public interface IEmailService
{
    public Task SendEmailAsync(string? email, string subject, string message);
}