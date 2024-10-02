using MediatR;

namespace NotificationService.SharedKernel.Interfaces;

/// <summary>
/// Represents a command that returns a response.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned by the command.</typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

/// <summary>
/// Represents a command that does not return a response.
/// </summary>
public interface ICommand : IRequest
{
}