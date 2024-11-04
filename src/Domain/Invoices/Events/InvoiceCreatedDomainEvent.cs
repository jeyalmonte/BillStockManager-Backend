using SharedKernel.Domain;

namespace Domain.Invoices.Events;
public sealed record InvoiceCreatedDomainEvent(Invoice Invoice) : IDomainEvent;
