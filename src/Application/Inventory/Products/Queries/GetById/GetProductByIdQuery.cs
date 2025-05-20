using Application.Inventory.Products.Contracts;
using SharedKernel.Interfaces.Messaging;

namespace Application.Inventory.Products.Queries.GetById;
public record GetProductByIdQuery(Guid Id) : IQuery<ProductResponse>;
