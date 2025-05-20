using Application.Billing.Invoices.Contracts;
using Domain.Billing;
using Domain.Inventory;
using SharedKernel.Results;

namespace Application.Billing.Invoices.Services.Interfaces;
public interface IInvoiceService
{
	Result<Success> AddInvoiceDetails(Invoice invoice, List<Product> products,
		List<CreateInvoiceDetailRequest> invoiceDetails);
}


