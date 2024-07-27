using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Users;

public static class UserErrors
{
    public static Error NotFound = new("User.NotFound", "User not found.");
    public static Error InvalidCredentials = new("User.InvalidCredentials", "Invalid credentials.");
}