using System.Reflection;
using CleanArchitecture.ArquitectureTests.Infrastructure;
using CleanArchitecture.Domain.Abstractions;
using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace CleanArchitecture.ArquitectureTests.Domain;
public class DomainTests : BaseTest
{
    [Fact]
    public void Entities_Should_Have_DefaultPrivateConstructorNoParams()
    {
        IEnumerable<Type> entities = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(Entity<>))
            .GetTypes();

        var errorEntities = new List<Type>();

        foreach (Type entityType in entities)
        {
            var constructors = entityType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            if (!constructors.Any(c => c.IsPrivate && c.GetParameters().Length == 0))
            {
                errorEntities.Add(entityType);
            }
        }

        errorEntities.Should().BeEmpty();
    }
}