using MediatR;
using SharedKernel.Results;

namespace SharedKernel.Interfaces.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
	where TQuery : IQuery<TResponse>
	where TResponse : notnull;


