using FluentValidation;

namespace Application.Inventory.Products.Commands.Create;
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .NotNull();

        RuleFor(x => x.Stock)
            .NotNull()
            .GreaterThan(0);
    }
}
