using Application.Inventory.Products.Queries.GetByFilter;
using Domain.Inventory;
using SharedKernel.Specification;
using System.Linq.Expressions;

namespace Application.Inventory.Products.Specifications;
internal class GetProductsByFilterSpecification : Specification<Product>
{
    public GetProductsByFilterSpecification(GetProductsByFilterQuery query)
    {
        if (query.Name is not null)
            AddCriteria(x => x.Name.Contains(query.Name));

        Expression<Func<Product, object>> sortBy =
            query.SortBy switch
            {
                "name" => x => x.Name,
                "price" => x => x.Price,
                _ => x => x.CreatedOn,
            };

        SetOrder(sortBy, query.OrderBy);
    }
}
