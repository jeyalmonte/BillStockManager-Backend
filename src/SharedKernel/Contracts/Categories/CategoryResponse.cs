namespace SharedKernel.Contracts.Categories;
public record CategoryResponse(
    Guid Id,
    string Name,
    string? Description
    );
