using SharedKernel.Domain;

namespace Domain.Products.Events;
public record ProductStockReducedEvent(Product Product, int Quantity) : IDomainEvent;
