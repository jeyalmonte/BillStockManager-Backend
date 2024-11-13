using SharedKernel.Contracts.Categories;
using SharedKernel.Interfaces.Messaging;

namespace Application.Inventory.Categories.Queries.GetAll;
public class GetAllCategoriesQuery : IQuery<ICollection<CategoryResponse>>;
