using Domain.Billing;
using Domain.Inventory;
using SharedKernel.Contracts.Invoices;
using SharedKernel.Results;

namespace Application.Billing.Invoices.Services.Interfaces;
public interface IInvoiceService
{
	Result<Success> AddInvoiceDetails(Invoice invoice, List<Product> products,
		List<CreateInvoiceDetailRequest> invoiceDetails);
}


