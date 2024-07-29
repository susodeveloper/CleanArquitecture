using CleanArchitecture.Application.Abstractions.Messaging;

namespace CleanArchitecture.Application.Users;

public record LoginCommand(string Email, string Password) : ICommand<string>;