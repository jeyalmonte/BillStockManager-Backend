using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Customers.Commands.Delete;
public record DeleteCustomerCommand(Guid Id) : ICommand<Success>;
