using MediatR;

public interface ICommand<TResponse> : IRequest<TResponse>
{
}