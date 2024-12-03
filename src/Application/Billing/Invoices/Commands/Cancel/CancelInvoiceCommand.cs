using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Billing.Invoices.Commands.Cancel;
public record CancelInvoiceCommand(Guid Id) : ICommand<Success>;