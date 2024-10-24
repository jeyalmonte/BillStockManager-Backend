using SharedKernel.Domain;

namespace Domain.Products.Events;
public record ProductStockIncreasedEvent(Guid ProductId, int Quantity) : IDomainEvent;
