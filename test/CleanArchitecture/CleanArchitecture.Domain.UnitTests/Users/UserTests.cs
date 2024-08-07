using CleanArchitecture.Domain.Roles;
using CleanArchitecture.Domain.UnitTests.Infrastructure;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Users.Events;
using FluentAssertions;
using Xunit;

namespace CleanArchitecture.Domain.UnitTests.Users;

public class UserTests : BaseTest
{
    [Fact]
    public void Create_ShouldSetPropertyValues()
    {

        // Arrange -> creada MockFIle -> UserMock.cs
       
        // Act
        var user = User.Create(UserMock.Nombre, UserMock.Apellido, UserMock.Email, UserMock.Password);

        // Assert
        user.Nombre.Should().Be(UserMock.Nombre);
        user.Apellido.Should().Be(UserMock.Apellido);
        user.Email.Should().Be(UserMock.Email);
        user.PasswordHash.Should().Be(UserMock.Password);
    }

    [Fact]
    public void Create_Should_RaiseUserCreateDomainEvent()
    {
        // Arrange
        var user = User.Create(UserMock.Nombre, UserMock.Apellido, UserMock.Email, UserMock.Password);

        // Act
        //var domainEvents = user.GetDomainEvents().OfType<UserCreatedDomainEvent>().SingleOrDefault();
        var domainEvents = AssertDomainEventWasPublished<UserCreatedDomainEvent>(user);

        // Assert
        domainEvents!.UserId.Should().Be(user.Id);
    }

    [Fact]
    public void Create_Should_AddRegisterRoleToUser()
    {
        // Arrange
        var user = User.Create(UserMock.Nombre, UserMock.Apellido, UserMock.Email, UserMock.Password);

        // Assert
        user.Roles.Should().NotBeNull();
        user.Roles.Should().Contain(Role.Cliente);
    }
}