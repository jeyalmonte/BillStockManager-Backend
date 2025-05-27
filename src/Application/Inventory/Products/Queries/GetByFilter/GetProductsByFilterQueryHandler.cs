using Application.Common.Results;
using Application.Inventory.Products.Contracts;
using Application.Inventory.Products.Specifications;
using Domain.Inventory.Repositories;
using Mapster;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Results;

namespace Application.Inventory.Products.Queries.GetByFilter;
public class GetProductsByFilterQueryHandler(IProductRepository productRepository)
	: IQueryHandler<GetProductsByFilterQuery, PaginatedResult<ProductResponse>>
{
	private readonly IProductRepository _productRepository = productRepository;
	public async Task<Result<PaginatedResult<ProductResponse>>> Handle(GetProductsByFilterQuery request, CancellationToken cancellationToken)
	{
		var specification = new GetProductsByFilterSpecification(request);

		var products = await _productRepository.GetAllBySpecAsync(
			specification: specification,
			pageSize: request.PageSize,
			pageNumber: request.PageNumber,
			cancellationToken: cancellationToken);

		var total = await _productRepository.GetTotalAsync(
			specification: specification,
			cancellationToken: cancellationToken);

		var productsResponse = products.Adapt<IReadOnlyCollection<ProductResponse>>();

		return PaginatedResult<ProductResponse>.Create(
			items: productsResponse,
			totalItems: total,
			pageNumber: request.PageNumber,
			pageSize: request.PageSize);
	}
}
