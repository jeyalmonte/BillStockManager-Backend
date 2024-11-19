using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Inventory.Products.Commands.UpdateStock;
public record UpdateProductStockCommand(
	Guid ProductId,
	int Quantity
	) : ICommand<Success>;
