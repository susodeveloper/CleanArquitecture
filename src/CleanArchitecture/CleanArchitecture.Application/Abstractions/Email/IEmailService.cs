
namespace CleanArchitecture.Application.Abstractions.Email
{
    public interface IEmailService
    {
        void Send(string receipient, string subject, string message);
    }
}