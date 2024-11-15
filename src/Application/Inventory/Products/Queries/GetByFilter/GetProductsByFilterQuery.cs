using Application.Common.Models;
using SharedKernel.Contracts.Products;
using SharedKernel.Enums;
using SharedKernel.Interfaces.Messaging;
using SharedKernel.Pagination;

namespace Application.Inventory.Products.Queries.GetByFilter;
public record GetProductsByFilterQuery(
    string? Name,
    string? SortBy,
    OrderType OrderBy = OrderType.Ascending
    ) : PaginatedQuery, IQuery<PaginatedResult<ProductResponse>>;
