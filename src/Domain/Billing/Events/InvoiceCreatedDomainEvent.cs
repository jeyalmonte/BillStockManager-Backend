using SharedKernel.Domain;

namespace Domain.Billing.Events;
public sealed record InvoiceCreatedDomainEvent(Invoice Invoice) : IDomainEvent;
