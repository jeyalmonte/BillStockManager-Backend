using FluentValidation;
using SharedKernel.Contracts.Invoices;

namespace Application.Billing.Invoices.Commands.Create;
public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
	public CreateInvoiceCommandValidator()
	{
		RuleFor(c => c.CustomerId).NotEmpty();
		RuleFor(c => c.InvoiceDetails).NotNull();
		RuleForEach(c => c.InvoiceDetails).SetValidator(new CreateInvoiceDetailRequestValidator());
	}

	private class CreateInvoiceDetailRequestValidator : AbstractValidator<CreateInvoiceDetailRequest>
	{
		public CreateInvoiceDetailRequestValidator()
		{
			RuleFor(c => c.ProductId).NotEmpty();
			RuleFor(c => c.Quantity).GreaterThan(0);
			RuleFor(c => c.Discount).InclusiveBetween(0, 100);
		}
	}
}
