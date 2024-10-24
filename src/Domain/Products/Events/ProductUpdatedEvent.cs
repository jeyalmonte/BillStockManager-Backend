using SharedKernel.Domain;

namespace Domain.Products.Events;
public record ProductUpdatedEvent(Product Product) : IDomainEvent;