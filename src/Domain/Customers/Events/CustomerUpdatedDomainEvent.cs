using SharedKernel.Domain;

namespace Domain.Customers.Events;
public sealed record CustomerUpdatedDomainEvent(Guid CustomerId) : IDomainEvent;
