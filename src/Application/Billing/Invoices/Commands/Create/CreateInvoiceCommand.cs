
using SharedKernel.Contracts.Invoices;
using SharedKernel.Interfaces.Messaging;

namespace Application.Billing.Invoices.Commands.Create;
public record CreateInvoiceCommand(
	Guid CustomerId,
	List<CreateInvoiceDetailRequest> InvoiceDetails
	) : ICommand<InvoiceResponse>;