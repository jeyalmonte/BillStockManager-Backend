using SharedKernel.Domain;

namespace Domain.Products.Events;
public record ProductRemovedEvent(Product Product) : IDomainEvent;
