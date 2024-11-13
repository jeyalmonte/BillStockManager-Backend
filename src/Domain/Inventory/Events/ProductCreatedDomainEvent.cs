using SharedKernel.Domain;

namespace Domain.Inventory.Events;
public sealed record ProductCreatedDomainEvent(Guid Id) : IDomainEvent;
