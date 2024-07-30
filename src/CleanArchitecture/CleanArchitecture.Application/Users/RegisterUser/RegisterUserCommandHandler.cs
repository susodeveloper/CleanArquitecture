using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users;
using MediatR;

namespace CleanArchitecture.Application.Users.RegisterUser;

internal class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userRepository.IsUserExistsAsync(new Email(request.Email));
        if (userExists) return Result.Failure<Guid>(UserErrors.AlreadyExists);

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = User.Create(
            new Nombre(request.Nombre),
            new Apellido(request.Apellidos),
            new Email(request.Email),
            new PasswordHash(passwordHash)
        );

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(user.Id!.Value);
    }
}