using SharedKernel.Domain;

namespace Domain.Inventory.Events;
public sealed record CategoryCreatedDomainEvent(Category Category) : IDomainEvent;

