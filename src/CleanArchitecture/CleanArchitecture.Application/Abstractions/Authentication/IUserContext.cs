namespace CleanArchitecture.Application.Abstractions.Authentication;
public interface IUserContext
{
    Guid UserId { get; }
    string UserEmail { get; }
}
