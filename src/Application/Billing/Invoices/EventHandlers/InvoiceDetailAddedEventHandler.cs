using Domain.Billing.Events;
using Domain.Inventory.Repositories;
using SharedKernel.Interfaces;
using SharedKernel.Interfaces.Messaging;

namespace Application.Billing.Invoices.EventHandlers;
internal class InvoiceDetailAddedEventHandler(
	IProductRepository productRepository,
	IUnitOfWork unitOfWork
	) : IEventHandler<InvoiceDetailAddedDomainEvent>
{
	public async Task Handle(InvoiceDetailAddedDomainEvent notification, CancellationToken cancellationToken)
	{
		var invoiceDetail = notification.InvoiceDetail;

		var product = await productRepository.GetByIdAsync(
			id: invoiceDetail.ProductId,
			asNoTracking: false,
			cancellationToken: cancellationToken) ?? throw new InvalidOperationException("Product not found.");

		int quantity = -invoiceDetail.Quantity;

		product.HandleStockChange(quantity);

		await unitOfWork.CommitAsync(cancellationToken);
	}
}
