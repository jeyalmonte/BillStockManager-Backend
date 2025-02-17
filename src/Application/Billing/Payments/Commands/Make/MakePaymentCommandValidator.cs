using Domain.Billing;
using FluentValidation;

namespace Application.Billing.Payments.Commands.Make;
public class MakePaymentCommandValidator : AbstractValidator<MakePaymentCommand>
{
	public MakePaymentCommandValidator()
	{
		RuleFor(x => x.InvoiceId).NotEmpty();
		RuleFor(x => x.Amount).GreaterThan(0);
		RuleFor(x => x.PaymentMethod).IsInEnum();
		RuleFor(x => x.ReferenceNumber).NotEmpty().When(x => x.PaymentMethod.Equals(PaymentMethod.Card));
		RuleFor(x => x.Currency).IsInEnum();
	}
}
