using SharedKernel.Contracts.Products;
using SharedKernel.Interfaces.Messaging;

namespace Application.Inventory.Products.Queries.GetById;
public record GetProductByIdQuery(Guid Id) : IQuery<ProductResponse>;
