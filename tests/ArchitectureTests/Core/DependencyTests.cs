using System.Reflection;
using NetArchTest.Rules;
using NotificationService.Application.Senders;

namespace NotificationService.ArchitectureTests.Core;

public class DependencyTests
{
    private static readonly Assembly CoreAssembly = typeof(EmailSender).Assembly;
    [Fact]
    public void Core_ShouldNotHaveDepdencyOnUpperLayers()
    {
        var result = Types
            .InAssembly(CoreAssembly)
            .ShouldNot()
            .HaveDependencyOnAny("NotificationService.Api")
            .GetResult();
        Assert.True(result.IsSuccessful);
    }
}