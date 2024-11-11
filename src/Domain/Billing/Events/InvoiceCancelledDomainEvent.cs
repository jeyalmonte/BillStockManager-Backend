using SharedKernel.Domain;

namespace Domain.Billing.Events;
public sealed record InvoiceCancelledDomainEvent(Guid InvoiceId) : IDomainEvent;
