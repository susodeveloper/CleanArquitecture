
namespace CleanArchitecture.Application.Abstractions.Email
{
    public interface IEmailService
    {
        Task SendAsync(Domain.Users.Email receipient, string subject, string message);
    }
}