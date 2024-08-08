using FluentValidation;

namespace CleanArchitecture.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("Nombre is required.");
        
        RuleFor(x => x.Apellidos)
            .NotEmpty().WithMessage("Apellidos is required.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email is not valid.");

        RuleFor(x => x.Password)
            .NotEmpty().MaximumLength(5).WithMessage("Password is required.");
    }
}