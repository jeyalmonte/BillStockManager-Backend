using Application.Inventory.Categories.Contracts;
using Domain.Inventory;
using Domain.Inventory.Repositories;
using Mapster;
using SharedKernel.Interfaces;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Inventory.Categories.Commands.Create;
public class CreateCategoryCommandHandler(
	ICategoryRepository categoryRepository,
	IUnitOfWork unitOfWork)
	: ICommandHandler<CreateCategoryCommand, CategoryResponse>
{
	private readonly ICategoryRepository _categoryRepository = categoryRepository;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	public async Task<Result<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
	{
		var existingCategory = await _categoryRepository.GetByNameAsync(
			name: request.Name,
			cancellationToken: cancellationToken
			);

		if (existingCategory is not null)
		{
			return Error.Conflict(description: "Category already exists.");
		}

		var newCategory = Category.Create(
			name: request.Name,
			description: request.Description
			);

		_categoryRepository.Add(newCategory);
		await _unitOfWork.CommitAsync(cancellationToken);

		return newCategory.Adapt<CategoryResponse>();
	}
}
