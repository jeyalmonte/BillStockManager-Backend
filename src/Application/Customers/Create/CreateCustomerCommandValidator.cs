using FluentValidation;

namespace Application.Customers.Create;
public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.DocumentType).IsInEnum();
        RuleFor(x => x.Document).NotEmpty();
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
