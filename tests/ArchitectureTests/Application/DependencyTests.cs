using System.Reflection;
using NetArchTest.Rules;

namespace NotificationService.ArchitectureTests.Application;

public class DependencyTests
{
    private static readonly Assembly ApplicationAssembly = Assembly.Load("NotificationService.Application");
    
    [Fact]
    public void ApplicationLayer_ShouldNotDependOnInfrastructure()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOnAny("NotificationService.Infrastructure")
            .GetResult();
        
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotDependOnApi()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOnAny("NotificationService.Api")
            .GetResult();
        
        Assert.True(result.IsSuccessful);
    }
}