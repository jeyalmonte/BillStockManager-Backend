using SharedKernel.Domain;

namespace Domain.Inventory.Events;
public sealed record ProductUpdatedDomainEvent(Product Product) : IDomainEvent;