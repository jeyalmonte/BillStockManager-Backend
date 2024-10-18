using SharedKernel.Domain;

namespace Domain.Products.Events;
public record ProductCreatedEvent(Product Product) : IDomainEvent;
