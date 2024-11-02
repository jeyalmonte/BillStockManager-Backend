﻿using FluentValidation;

namespace Application.Identity.Commands.Login;
public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
	public LoginUserCommandValidator()
	{
		RuleFor(x => x.Username).NotEmpty();
		RuleFor(x => x.Password).NotEmpty();
	}
}