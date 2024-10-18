using SharedKernel.Domain;

namespace Domain.Invoices.Events;
public record InvoicePaidEvent(Guid InvoiceId) : IDomainEvent;
