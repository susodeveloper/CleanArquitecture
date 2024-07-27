using CleanArchitecture.Application.Abstractions.Email;

namespace CleanArchitecture.Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    public Task SendAsync(Domain.Users.Email receipient, string subject, string message)
    {
        return Task.CompletedTask;
    }
}