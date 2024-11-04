using SharedKernel.Domain;
namespace Domain.Products.Events;
public sealed record ProductRemovedDomainEvent(Product Product) : IDomainEvent;
