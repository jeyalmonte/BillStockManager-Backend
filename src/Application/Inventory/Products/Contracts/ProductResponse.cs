namespace Application.Inventory.Products.Contracts;
public record ProductResponse(
    Guid Id,
    string Name,
    Guid CategoryId,
    string? Description,
    decimal Price,
    int Stock,
    decimal? Discount
    );
