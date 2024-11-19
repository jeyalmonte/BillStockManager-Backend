namespace SharedKernel.Contracts.Products;
public record UpdateProductRequest(
	string Name,
	string? Description,
	Guid CategoryId,
	decimal Price,
	decimal? Discount
	);
