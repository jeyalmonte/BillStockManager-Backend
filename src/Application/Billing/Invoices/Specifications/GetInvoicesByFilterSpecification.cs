using Application.Billing.Invoices.Queries.GetByFilter;
using Domain.Billing;
using SharedKernel.Specification;
using System.Linq.Expressions;

namespace Application.Billing.Invoices.Specifications;
internal class GetInvoicesByFilterSpecification : Specification<Invoice>
{
	public GetInvoicesByFilterSpecification(GetInvoicesByFilterQuery query)
	{
		if (query.InvoiceNumber is not null)
			AddCriteria(x => x.InvoiceNumber == query.InvoiceNumber);

		if (query.CustomerName is not null)
			AddCriteria(x => x.Customer.FullName.Contains(query.CustomerName));

		if (query.Status is not null)
			AddCriteria(x => x.Status == query.Status);

		Expression<Func<Invoice, object>> sortBy = query.SortBy switch
		{
			"invoicenumber" => x => x.InvoiceNumber,
			"totalamount" => x => x.TotalAmount,
			"status" => x => x.Status,
			_ => x => x.CreatedOn
		};

		SetOrder(sortBy, query.OrderBy);
	}
}
