using SharedKernel.Domain;

namespace Domain.Products.Events;
public sealed record ProductStockIncreasedDomainEvent(Guid ProductId, int Quantity) : IDomainEvent;
