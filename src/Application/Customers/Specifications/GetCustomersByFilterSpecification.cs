using Application.Customers.Queries.GetByFilter;
using Domain.Customers;
using SharedKernel.Specification;
using System.Linq.Expressions;

namespace Application.Customers.Specifications;
public class GetCustomersByFilterSpecification : Specification<Customer>
{
    public GetCustomersByFilterSpecification(GetCustomersByFilterQuery query)
    {
        if (query.FullName is not null)
            AddCriteria(x => x.FullName.Contains(query.FullName));

        if (query.Nickname is not null)
            AddCriteria(x => x.Nickname != null && x.Nickname.Contains(query.Nickname));

        if (query.DocumentType.HasValue)
            AddCriteria(x => x.DocumentType == query.DocumentType);

        if (query.Document is not null)
            AddCriteria(x => x.Document == query.Document);

        if (query.Gender.HasValue)
            AddCriteria(x => x.Gender == query.Gender);

        Expression<Func<Customer, object>> sortBy =
            query.SortBy switch
            {
                "fullname" => customer => customer.FullName,
                "gender" => customer => customer.Gender,
                _ => customer => customer.CreatedOn
            };

        SetOrder(sortBy, query.OrderBy);
    }
}

