using MediatR;

namespace NotificationService.SharedKernel.Interfaces;

/// <summary>
/// Represents a handler for processing queries of type <typeparamref name="TQuery"/> that return a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TQuery">The type of the query that this handler processes.</typeparam>
/// <typeparam name="TResponse">The type of the response that this query returns.</typeparam>
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}