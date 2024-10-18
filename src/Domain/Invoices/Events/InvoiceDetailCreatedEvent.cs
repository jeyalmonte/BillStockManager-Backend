using SharedKernel.Domain;

namespace Domain.Invoices.Events;
public record InvoiceDetailCreatedEvent(InvoiceDetail InvoiceDetail) : IDomainEvent;
