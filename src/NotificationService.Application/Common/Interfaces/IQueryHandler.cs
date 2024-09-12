using MediatR;

namespace NotificationService.Application.Common.Interfaces;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}