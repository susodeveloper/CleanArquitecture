using CleanArchitecture.Application.Abstractions.Email;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Users.Events;
using MediatR;

namespace CleanArchitecture.Application.Users.RegisterUser;

internal sealed class UserCreatedDomainEventHnadler : INotificationHandler<UserCreatedDomainEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public UserCreatedDomainEventHnadler(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken);

        if(user is null) return;

        _emailService.Send(user.Email!.Value!, "Welcome", "Welcome to our platform");

    }
}
