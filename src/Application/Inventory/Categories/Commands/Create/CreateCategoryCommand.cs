using Application.Inventory.Categories.Contracts;
using SharedKernel.Interfaces.Messaging;

namespace Application.Inventory.Categories.Commands.Create;
public record CreateCategoryCommand(
    string Name,
    string? Description
    ) : ICommand<CategoryResponse>;