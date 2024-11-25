using SharedKernel.Contracts.Invoices;
using SharedKernel.Interfaces.Messaging;

namespace Application.Billing.Invoices.Queries.GetById;
public record GetInvoiceByIdQuery(Guid Id) : IQuery<InvoiceResponse>;
