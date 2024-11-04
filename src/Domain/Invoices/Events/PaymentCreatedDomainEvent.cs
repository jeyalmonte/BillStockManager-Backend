using SharedKernel.Domain;

namespace Domain.Invoices.Events;
public sealed record PaymentCreatedDomainEvent(Payment Transaction) : IDomainEvent;
