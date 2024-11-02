using FluentValidation;

namespace Application.Identity.Commands.Register;
public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
	public RegisterUserCommandValidator()
	{
		RuleFor(x => x.Name).NotEmpty();
		RuleFor(x => x.Username).NotEmpty();
		RuleFor(x => x.Email).NotEmpty().EmailAddress();
		RuleFor(x => x.Password).NotEmpty().Length(8, 20);
	}
}
