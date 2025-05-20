using Application.Billing.Invoices.Contracts;
using Application.Billing.Invoices.Services.Interfaces;
using Domain.Billing;
using Domain.Billing.Repositories;
using Domain.Customers.Repositories;
using Domain.Inventory.Repositories;
using Mapster;
using SharedKernel.Interfaces;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Billing.Invoices.Commands.Create;
public class CreateInvoiceCommandHandler(
	IInvoiceService invoiceService,
	IInvoiceRepository invoiceRepository,
	IProductRepository productRepository,
	ICustomerRepository customerRepository,
	IUnitOfWork unitOfWork)
	: ICommandHandler<CreateInvoiceCommand, InvoiceResponse>
{
	public async Task<Result<InvoiceResponse>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
	{
		var customer = await customerRepository.GetByIdAsync(
			id: request.CustomerId,
			cancellationToken: cancellationToken);

		if (customer is null)
		{
			return Error.Failure(description: "Customer not found.");
		}

		var invoice = Invoice.Create(customer.Id);

		var productIds = request.InvoiceDetails.Select(d => d.ProductId).ToList();
		var products = await productRepository.GetByIdsAsync(
			ids: productIds,
			cancellationToken: cancellationToken
		);

		if (products.Count != productIds.Count)
		{
			return Error.Failure(description: "Some products not found.");
		}

		var invoiceDetailsResult = invoiceService.AddInvoiceDetails(
			invoice: invoice,
			products: [.. products],
			invoiceDetails: request.InvoiceDetails
		);

		if (invoiceDetailsResult.HasError)
		{
			return invoiceDetailsResult.Errors;
		}

		invoiceRepository.Add(invoice);
		await unitOfWork.CommitAsync(cancellationToken);

		var invoiceResponse = invoice.Adapt<InvoiceResponse>();

		invoiceResponse = invoiceResponse with
		{
			Customer = customer.Adapt<CustomerInvoiceDto>()
		};

		return invoiceResponse;
	}
}