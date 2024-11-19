using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Inventory.Products.Commands.Delete;
public record DeleteProductCommand(Guid Id) : ICommand<Success>;
