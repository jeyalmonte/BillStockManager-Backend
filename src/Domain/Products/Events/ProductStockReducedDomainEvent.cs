using SharedKernel.Domain;

namespace Domain.Products.Events;
public sealed record ProductStockReducedDomainEvent(Guid ProductId, int Quantity) : IDomainEvent;
