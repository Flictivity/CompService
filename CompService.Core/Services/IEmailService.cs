using CompService.Core.Results;

namespace CompService.Core.Services;

public interface IEmailService
{
    public Task<BaseResult> SendEmailAsync(string? email, string subject, string message);
}