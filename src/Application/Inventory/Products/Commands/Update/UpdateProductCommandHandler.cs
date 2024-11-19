using Domain.Inventory.Repositories;
using SharedKernel.Interfaces;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Inventory.Products.Commands.Update;
public class UpdateProductCommandHandler(
	IProductRepository productRepository,
	ICategoryRepository categoryRepository,
	IUnitOfWork unitOfWork
	) : ICommandHandler<UpdateProductCommand, Success>
{
	public async Task<Result<Success>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
	{
		var product = await productRepository.GetByIdAsync(
			id: request.Id,
			asNoTracking: false,
			cancellationToken: cancellationToken);

		if (product is null)
		{
			return Error.NotFound(description: "Product not found.");
		}

		var existingCategory = await categoryRepository.GetByIdAsync(
			id: request.CategoryId,
			cancellationToken: cancellationToken);

		if (existingCategory is null)
		{
			return Error.Failure(description: "Category not found.");
		}

		product.Update(
			name: request.Name,
			description: request.Description,
			categoryId: request.CategoryId,
			price: request.Price,
			discount: request.Discount);

		await unitOfWork.CommitAsync(cancellationToken);

		return Result.Success;
	}
}
