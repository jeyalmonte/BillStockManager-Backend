using SharedKernel.Domain;

namespace Domain.Invoices.Events;
public record PaymentCreatedEvent(Payment Transaction) : IDomainEvent;
