using MediatR;

namespace NotificationService.Application.Common.Interfaces;

public interface IQuery<TResponse> : IRequest<TResponse>
{
}