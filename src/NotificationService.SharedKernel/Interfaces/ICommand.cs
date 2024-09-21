using MediatR;

namespace NotificationService.SharedKernel.Interfaces;

public interface ICommand<TResponse> : IRequest<TResponse>
{
}

public interface ICommand : IRequest
{
}