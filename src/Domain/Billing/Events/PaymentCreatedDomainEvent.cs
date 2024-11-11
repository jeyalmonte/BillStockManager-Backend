using SharedKernel.Domain;

namespace Domain.Billing.Events;
public sealed record PaymentCreatedDomainEvent(Payment Transaction) : IDomainEvent;
