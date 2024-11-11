using SharedKernel.Domain;
namespace Domain.Inventory.Events;
public sealed record ProductRemovedDomainEvent(Product Product) : IDomainEvent;
