using FluentValidation;

namespace Application.Customers.Commands.Update;
public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
	public UpdateCustomerCommandValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty();

		RuleFor(x => x.FullName)
			.NotEmpty();

		RuleFor(x => x.Gender).IsInEnum();

		RuleFor(x => x.Email)
			.EmailAddress()
			.When(x => !string.IsNullOrEmpty(x.Email))
			.WithMessage("Please provide a valid email address.");

		RuleFor(x => x.PhoneNumber)
			.MinimumLength(10)
			.When(x => !string.IsNullOrEmpty(x.PhoneNumber));
	}
}
