using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Inventory.Products.Commands.ReduceStock;
public record ReduceProductStockCommand(
	Guid ProductId,
	int Quantity
	) : ICommand<Success>;
