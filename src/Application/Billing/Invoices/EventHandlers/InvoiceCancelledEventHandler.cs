using Domain.Billing.Events;
using Domain.Billing.Repositories;
using Domain.Inventory.Repositories;
using SharedKernel.Interfaces;
using SharedKernel.Interfaces.Messaging;

namespace Application.Billing.Invoices.EventHandlers;
internal class InvoiceCancelledEventHandler(
	IInvoiceRepository invoiceRepository,
	IInvoiceDetailRepository invoiceDetailRepository,
	IProductRepository productRepository,
	IUnitOfWork unitOfWork
	) : IEventHandler<InvoiceCancelledDomainEvent>
{
	public async Task Handle(InvoiceCancelledDomainEvent notification, CancellationToken cancellationToken)
	{
		var invoice = await invoiceRepository.GetByIdAsync(
			id: notification.InvoiceId,
			asNoTracking: false,
			cancellationToken: cancellationToken) ?? throw new InvalidOperationException("Invoice not found.");

		invoice.MarkAsDeleted();

		var details = await invoiceDetailRepository.GetDetailsByInvoiceIdAsync(
			invoiceId: notification.InvoiceId,
			cancellationToken: cancellationToken);

		var productIds = details.Select(x => x.ProductId).ToList();
		var products = await productRepository.GetByIdsAsync(
			ids: productIds,
			asNoTracking: false,
			cancellationToken: cancellationToken);

		foreach (var detail in details)
		{
			var product = products.SingleOrDefault(x => x.Id == detail.ProductId)
				?? throw new InvalidOperationException("Product not found.");

			product.HandleStockChange(detail.Quantity);
			detail.MarkAsDeleted();
		}

		await unitOfWork.CommitAsync(cancellationToken);
	}
}
