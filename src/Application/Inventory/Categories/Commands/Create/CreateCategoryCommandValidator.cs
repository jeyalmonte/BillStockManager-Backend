using FluentValidation;

namespace Application.Inventory.Categories.Commands.Create;
public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
