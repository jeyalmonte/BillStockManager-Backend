using Application.Common.Results;
using Application.Inventory.Products.Contracts;
using SharedKernel.Enums;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Pagination;

namespace Application.Inventory.Products.Queries.GetByFilter;
public record GetProductsByFilterQuery(
    string? Name,
    string? SortBy,
    OrderType OrderBy = OrderType.Ascending
    ) : PaginatedQuery, IQuery<PaginatedResult<ProductResponse>>;
