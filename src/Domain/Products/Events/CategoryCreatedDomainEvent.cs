using SharedKernel.Domain;

namespace Domain.Products.Events;
public sealed record CategoryCreatedDomainEvent(Category Category) : IDomainEvent;

