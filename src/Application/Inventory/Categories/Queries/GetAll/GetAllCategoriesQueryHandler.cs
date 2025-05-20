using Application.Inventory.Categories.Contracts;
using Domain.Inventory.Repositories;
using Mapster;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Inventory.Categories.Queries.GetAll;
public class GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository) : IQueryHandler<GetAllCategoriesQuery, ICollection<CategoryResponse>>
{
	public async Task<Result<ICollection<CategoryResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
	{
		var categories = await categoryRepository.GetAllAsync(cancellationToken);

		return categories.Adapt<List<CategoryResponse>>();
	}
}
