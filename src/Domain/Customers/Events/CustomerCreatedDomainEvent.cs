using SharedKernel.Domain;

namespace Domain.Customers.Events;
public sealed record CustomerCreatedDomainEvent(Customer Customer) : IDomainEvent;
