using SharedKernel.Domain;

namespace Domain.Invoices.Events;
public record TransactionCreatedEvent(Transaction Transaction) : IDomainEvent;
