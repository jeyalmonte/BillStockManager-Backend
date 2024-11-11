using SharedKernel.Domain;

namespace Domain.Billing.Events;
public sealed record InvoicePaidDomainEvent(Guid InvoiceId) : IDomainEvent;
