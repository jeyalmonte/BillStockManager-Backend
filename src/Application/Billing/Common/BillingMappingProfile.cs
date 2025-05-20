using Application.Billing.Invoices.Contracts;
using Domain.Billing;
using Domain.Customers;
using Mapster;

namespace Application.Billing.Common;
public class BillingMappingProfile : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Invoice, InvoiceResponse>()
			.Map(dest => dest.InvoiceDate, src => src.CreatedOn);

		config.NewConfig<CustomerInvoiceDto, Customer>();
		config.NewConfig<InvoiceDetail, InvoiceDetailResponse>();
	}
}
