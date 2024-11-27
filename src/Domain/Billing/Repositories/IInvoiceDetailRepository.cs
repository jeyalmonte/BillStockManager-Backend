namespace Domain.Billing.Repositories;
public interface IInvoiceDetailRepository
{
	Task<ICollection<InvoiceDetail>> GetDetailsByInvoiceId(Guid invoiceId,
		CancellationToken cancellationToken = default);
}
