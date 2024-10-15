using MediatR;
using SharedKernel.Results;

namespace SharedKernel.Interfaces.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
	where TResponse : notnull;

