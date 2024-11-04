using SharedKernel.Domain;

namespace Domain.Invoices.Events;
public sealed record InvoiceCancelledDomainEvent(Guid InvoiceId) : IDomainEvent;
