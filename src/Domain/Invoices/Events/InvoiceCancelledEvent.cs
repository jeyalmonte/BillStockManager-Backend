using SharedKernel.Domain;

namespace Domain.Invoices.Events;
public record InvoiceCancelledEvent(Guid InvoiceId) : IDomainEvent;
