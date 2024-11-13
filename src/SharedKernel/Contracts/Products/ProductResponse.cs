namespace SharedKernel.Contracts.Products;
public record ProductResponse(
    Guid Id,
    string Name,
    Guid CategoryId,
    string? Description,
    decimal Price,
    int Stock,
    decimal? Discount
    );
