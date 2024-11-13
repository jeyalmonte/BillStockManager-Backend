using SharedKernel.Contracts.Products;
using SharedKernel.Interfaces.Messaging;

namespace Application.Inventory.Products.Commands.Create;
public record CreateProductCommand(
    string Name,
    Guid CategoryId,
    string? Description,
    decimal Price,
    int Stock,
    decimal? Discount
    ) : ICommand<ProductResponse>;
