using SharedKernel.Domain;

namespace Domain.Inventory.Events;
public sealed record ProductStockChangedDomainEvent(Guid Id, int Quantity) : IDomainEvent;
