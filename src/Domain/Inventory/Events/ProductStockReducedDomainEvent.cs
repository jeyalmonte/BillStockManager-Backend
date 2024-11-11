using SharedKernel.Domain;

namespace Domain.Inventory.Events;
public sealed record ProductStockReducedDomainEvent(Guid ProductId, int Quantity) : IDomainEvent;
