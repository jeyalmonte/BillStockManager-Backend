using Domain.Billing;
using Mapster;
using SharedKernel.Contracts.Invoices;

namespace Application.Billing.Common;
public class BillingMappingProfile : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Invoice, InvoiceResponse>()
			.Map(dest => dest.InvoiceDate, src => src.CreatedOn);
	}
}
