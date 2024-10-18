using SharedKernel.Domain;

namespace Domain.Customers.Events;
public record CustomerCreatedEvent(Customer Customer) : IDomainEvent;
