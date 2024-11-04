using SharedKernel.Domain;

namespace Domain.Invoices.Events;
public sealed record InvoiceDetailAddedDomainEvent(InvoiceDetail InvoiceDetail) : IDomainEvent;
