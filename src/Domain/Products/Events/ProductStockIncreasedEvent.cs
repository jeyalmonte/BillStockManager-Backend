using SharedKernel.Domain;

namespace Domain.Products.Events;
public record ProductStockIncreasedEvent(Product Product, int Quantity) : IDomainEvent;
