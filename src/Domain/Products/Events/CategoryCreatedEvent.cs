using SharedKernel.Domain;

namespace Domain.Products.Events;
public record CategoryCreatedEvent(Category Category) : IDomainEvent;

