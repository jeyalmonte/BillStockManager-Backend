using SharedKernel.Domain;

namespace Domain.Inventory.Events;
public sealed record ProductStockIncreasedDomainEvent(Guid ProductId, int Quantity) : IDomainEvent;
