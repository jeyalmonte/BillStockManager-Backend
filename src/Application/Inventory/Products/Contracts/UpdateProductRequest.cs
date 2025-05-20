namespace Application.Inventory.Products.Contracts;
public record UpdateProductRequest(
	string Name,
	string? Description,
	Guid CategoryId,
	decimal Price,
	decimal? Discount
	);
