using Application.Inventory.Categories.Contracts;
using SharedKernel.Interfaces.Messaging;

namespace Application.Inventory.Categories.Queries.GetAll;
public class GetAllCategoriesQuery : IQuery<ICollection<CategoryResponse>>;
