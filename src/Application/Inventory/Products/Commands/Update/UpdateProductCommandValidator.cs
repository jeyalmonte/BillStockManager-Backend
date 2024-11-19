using FluentValidation;

namespace Application.Inventory.Products.Commands.Update;
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
	public UpdateProductCommandValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.Name).NotEmpty();
		RuleFor(x => x.CategoryId).NotEmpty();
		RuleFor(x => x.Price).GreaterThan(0);
		RuleFor(x => x.Discount)
			.GreaterThanOrEqualTo(0)
			.LessThanOrEqualTo(100);
	}
}
