using SharedKernel.Domain;

namespace Domain.Invoices.Events;
public sealed record InvoicePaidDomainEvent(Guid InvoiceId) : IDomainEvent;
