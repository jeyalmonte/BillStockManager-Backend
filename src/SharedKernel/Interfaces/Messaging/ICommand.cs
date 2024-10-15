using MediatR;
using SharedKernel.Results;

namespace SharedKernel.Interfaces.Messaging;

public interface ICommand : IRequest;
public interface ICommand<Response> : IRequest<Result<Response>>;

