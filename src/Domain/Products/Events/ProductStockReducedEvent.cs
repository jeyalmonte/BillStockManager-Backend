using SharedKernel.Domain;

namespace Domain.Products.Events;
public record ProductStockReducedEvent(Guid ProductId, int Quantity) : IDomainEvent;
