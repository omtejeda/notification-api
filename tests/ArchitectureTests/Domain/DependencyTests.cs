using System.Reflection;
using NetArchTest.Rules;

namespace NotificationService.ArchitectureTests.Domain;

public class DependencyTests
{
    private static readonly Assembly DomainAssembly = Assembly.Load("NotificationService.Domain");
    
    [Fact]
    public void Domain_ShouldNotHaveDepedencyOnExternalLayers()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny("NotificationService.Api", "NotificationService.Infrastructure", "NotificationService.Application")
            .GetResult();
        
        Assert.True(result.IsSuccessful);
    }
}