using MediatR;

namespace NotificationService.SharedKernel.Interfaces;

/// <summary>
/// Represents a query that returns a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TResponse">The type of the response that this query returns.</typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}

/// <summary>
/// Represents a query with the owner information.
/// </summary>
public abstract record OwnedQuery
{
    private string? _owner;

    /// <summary>
    /// Gets the owner of the query.
    /// </summary>
    /// <returns>The owner of the query as a string, or null if not set.</returns>
    public string? GetOwner() => _owner;

    /// <summary>
    /// Sets the owner of the query.
    /// </summary>
    /// <param name="owner">The owner of the query as a string.</param>
    public void SetOwner(string? owner) => _owner = owner;
}