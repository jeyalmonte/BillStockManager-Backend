namespace Domain.Billing.Repositories;
public interface IInvoiceDetailRepository
{
	Task<ICollection<InvoiceDetail>> GetDetailsWithProductByInvoiceIdAsync(Guid invoiceId,
		CancellationToken cancellationToken = default);
}
