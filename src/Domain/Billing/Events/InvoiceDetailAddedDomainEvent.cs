using SharedKernel.Domain;

namespace Domain.Billing.Events;
public sealed record InvoiceDetailAddedDomainEvent(InvoiceDetail InvoiceDetail) : IDomainEvent;
