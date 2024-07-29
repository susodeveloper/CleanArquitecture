using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Users;
public sealed class User : Entity<UserId>
{
    private User(){}
    private User(
        UserId id,
        Nombre nombre,
        Apellido apellido,
        Email email
    ): base(id) 
    {
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
    }

    public Nombre? Nombre { get; private set; }
    public Apellido? Apellido { get; private set; }
    public Email? Email { get; private set; }

    public static User Create(
        Nombre nombre,
        Apellido apellido,
        Email email
    )
    {
        var user = new User(UserId.New(), nombre, apellido, email);
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id!));
        return user;
    }
}

