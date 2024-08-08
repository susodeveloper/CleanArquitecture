using CleanArchitecture.ArquitectureTests.Infrastructure;
using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace CleanArchitecture.ArquitectureTests.Layers;
public class LayerTests : BaseTest
{
    [Fact]
    public void DomainLayer_ShouldHaveNotDependency_ApplicationLayer()
    {
        var results = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
            .GetResult();

        results.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainLayer_ShouldHaveNotDependency_InfrastructureLayer()
    {
        var results = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        results.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ApplicationLayer_ShouldHaveNotDependency_InfrastructureLayer()
    {
        var results = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        results.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainLayer_ShouldHaveNotDependency_PresentantionLayer()
    {
        var results = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        results.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void InfrastructureLayer_ShouldHaveNotDependency_PresentationLayer()
    {
        var results = Types.InAssembly(InfrastructureAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        results.IsSuccessful.Should().BeTrue();
    }
}