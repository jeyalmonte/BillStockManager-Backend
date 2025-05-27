using Application.Inventory.Products.Contracts;
using Domain.Inventory;
using Domain.Inventory.Repositories;
using Mapster;
using SharedKernel.Interfaces;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Inventory.Products.Commands.Create;
public class CreateProductCommandHandler(
	IProductRepository productRepository,
	ICategoryRepository categoryRepository,
	IUnitOfWork unitOfWork
	)
	: ICommandHandler<CreateProductCommand, ProductResponse>
{
	private readonly IProductRepository _productRepository = productRepository;
	private readonly ICategoryRepository _categoryRepository = categoryRepository;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public async Task<Result<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
	{
		var existingCategory = await _categoryRepository.GetByIdAsync(
			 id: request.CategoryId,
			 cancellationToken: cancellationToken);

		if (existingCategory is null)
		{
			return Error.NotFound(description: "Category not found.");
		}

		var productWithSameName = await _productRepository.GetByNameAsync(
			name: request.Name,
			cancellationToken: cancellationToken);

		if (productWithSameName is not null)
		{
			return Error.Conflict(description: $"Product with name '{request.Name}' already exists.");
		}

		var newProduct = Product.Create(
			name: request.Name,
			categoryId: request.CategoryId,
			description: request.Description,
			price: request.Price,
			stock: request.Stock,
			discount: request.Discount
			);

		_productRepository.Add(newProduct);
		await _unitOfWork.CommitAsync(cancellationToken);

		return newProduct.Adapt<ProductResponse>();
	}
}
