using Application.Billing.Invoices.Services.Interfaces;
using Domain.Billing;
using Domain.Inventory;
using SharedKernel.Contracts.Invoices;
using SharedKernel.Results;

namespace Application.Billing.Invoices.Services;
internal class InvoiceService : IInvoiceService
{
	public Result<Success> AddInvoiceDetails(Invoice invoice, List<Product> products,
		List<CreateInvoiceDetailRequest> invoiceDetails)
	{
		foreach (var detail in invoiceDetails)
		{
			var product = products.First(p => p.Id == detail.ProductId);

			if (product.Stock < detail.Quantity)
			{
				return Error.Failure(description: $"Product {product.Id} does not have enough stock.");
			}

			var invoiceDetail = InvoiceDetail.Create(
				invoiceId: invoice.Id,
				product: product,
				quantity: detail.Quantity,
				discount: detail.Discount);

			if (invoiceDetail.HasError)
			{
				return invoiceDetail.Errors;
			}

			var invoiceDetailResult = invoice.AddInvoiceDetail(invoiceDetail.Value);

			if (invoiceDetailResult.HasError)
			{
				return invoiceDetailResult.Errors;
			}
		}

		return Result.Success;
	}

}
