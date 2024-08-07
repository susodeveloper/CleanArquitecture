using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.UnitTests.Users;
public static class UserMock
{
    public static readonly Domain.Users.Nombre Nombre = new("Eduardo");
    public static readonly Apellido Apellido = new("Garcia");
    public static readonly Email Email = new("eduardo.garcia@gmail.com");
    public static readonly PasswordHash Password = new("AfED%%32111");
    
    public static User Create()
    {
        return User.Create(Nombre, Apellido, Email, Password);
    }
}