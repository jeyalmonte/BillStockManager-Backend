using SharedKernel.Domain;

namespace Domain.Products.Events;
public record ProductAddedToCategoryEvent(Guid CategoryId, Product Product) : IDomainEvent;
