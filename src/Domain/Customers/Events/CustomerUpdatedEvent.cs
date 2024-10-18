using SharedKernel.Domain;

namespace Domain.Customers.Events;
public record CustomerUpdatedEvent(Guid CustomerId) : IDomainEvent;
