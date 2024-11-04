using SharedKernel.Domain;

namespace Domain.Products.Events;
public sealed record ProductUpdatedDomainEvent(Product Product) : IDomainEvent;