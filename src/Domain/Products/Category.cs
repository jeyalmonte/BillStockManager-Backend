using Domain.Products.Events;
using SharedKernel.Domain;
using SharedKernel.Results;

namespace Domain.Products;
public sealed class Category : BaseAuditableEntity
{
	private readonly List<Product> _products = [];
	public string Name { get; private set; } = null!;
	public string? Description { get; private set; }
	public IReadOnlyList<Product> Products => _products.AsReadOnly();

	public Category(string name, string? description)
	{
		Name = name;
		Description = description;
	}

	public static Category Create(string name, string? description)
	{
		var category = new Category(
			name: name,
			description: description);

		category.RaiseEvent(new CategoryCreatedEvent(category));

		return category;
	}

	public Result<Success> AddProduct(Product product)
	{
		if (_products.Any(p => p.Name == product.Name))
		{
			Error.Conflict(description: $"Product {product.Name} already exists in the category.");
		}

		_products.Add(product);

		return Result.Success;
	}

	private Category() { }
}
