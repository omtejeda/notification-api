using MediatR;

namespace NotificationService.Application.Common.Interfaces;

public interface ICommand<TResponse> : IRequest<TResponse>
{
}

public interface ICommand : IRequest
{
}