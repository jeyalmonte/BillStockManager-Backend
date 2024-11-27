using SharedKernel.Contracts.Invoices;
using SharedKernel.Interfaces.Messaging;

namespace Application.Billing.Invoices.Queries.GetDetails;
public record GetInvoiceDetailsQuery(Guid Id) : IQuery<ICollection<InvoiceDetailResponse>>;
