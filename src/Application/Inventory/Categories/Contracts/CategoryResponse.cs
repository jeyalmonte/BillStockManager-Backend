namespace Application.Inventory.Categories.Contracts;
public record CategoryResponse(
	Guid Id,
	string Name,
	string? Description
	);
