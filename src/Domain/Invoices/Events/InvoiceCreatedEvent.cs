using SharedKernel.Domain;

namespace Domain.Invoices.Events;
public record InvoiceCreatedEvent(Invoice Invoice) : IDomainEvent;
