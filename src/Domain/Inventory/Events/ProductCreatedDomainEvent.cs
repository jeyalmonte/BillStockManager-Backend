using SharedKernel.Domain;

namespace Domain.Inventory.Events;
public record ProductCreatedDomainEvent(Guid Id) : IDomainEvent;
