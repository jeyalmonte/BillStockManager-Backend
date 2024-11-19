using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Inventory.Products.Commands.Update;
public record UpdateProductCommand(
	Guid Id,
	string Name,
	string? Description,
	Guid CategoryId,
	decimal Price,
	decimal? Discount
	) : ICommand<Success>;
