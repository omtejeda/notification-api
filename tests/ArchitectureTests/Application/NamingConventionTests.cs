using System.Reflection;
using NetArchTest.Rules;

namespace NotificationService.ArchitectureTests.Application;

public class NamingConventionTests
{
    private static readonly Assembly ApplicationAssembly = Assembly.Load("NotificationService.Application");

    [Fact]
    public void AllCommands_ShouldEndWithCommand()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(NotificationService.SharedKernel.Interfaces.ICommand))
            .Should()
            .HaveNameEndingWith("Command")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void AllQueries_ShouldEndWithQuery()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(NotificationService.SharedKernel.Interfaces.IQuery<>))
            .Should()
            .HaveNameEndingWith("Query")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void AllQueryHandlers_ShouldEndWithQueryHandler()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(NotificationService.SharedKernel.Interfaces.IQueryHandler<,>))
            .Should()
            .HaveNameEndingWith("QueryHandler")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void AllCommandHandlers_ShouldEndWithCommandHandler()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(NotificationService.SharedKernel.Interfaces.ICommandHandler<>))
            .Should()
            .HaveNameEndingWith("CommandHandler")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void AllEvents_ShouldEndWithEvent()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(NotificationService.SharedKernel.Interfaces.IEvent))
            .Should()
            .HaveNameEndingWith("Event")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void AllEventHandlers_ShouldEndWithEventHandler()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(NotificationService.SharedKernel.Interfaces.IEventHandler<>))
            .Should()
            .HaveNameEndingWith("EventHandler")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void AllInterfaces_ShouldStartWithPreffixI()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .AreInterfaces()
            .Should()
            .HaveNameStartingWith("I")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
}
