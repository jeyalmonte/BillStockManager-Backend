using SharedKernel.Domain;

namespace Domain.Invoices.Events;
public record InvoiceDetailAddedEvent(InvoiceDetail InvoiceDetail) : IDomainEvent;
