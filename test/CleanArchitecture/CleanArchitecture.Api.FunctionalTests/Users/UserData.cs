using CleanArchitecture.Application.Users.RegisterUser;

namespace CleanArchitecture.Api.FunctionalTests.Users;

internal static class UserData
{
    public static RegisterUserRequest RegisterUserRequest => new("felipe.rosas@test.com", "Felipe", "Rosas", "123456");
    
}