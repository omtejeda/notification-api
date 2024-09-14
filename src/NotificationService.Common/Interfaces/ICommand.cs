using MediatR;

namespace NotificationService.Common.Interfaces;

public interface ICommand<TResponse> : IRequest<TResponse>
{
}

public interface ICommand : IRequest
{
}