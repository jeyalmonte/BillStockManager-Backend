using Domain.Products.Events;
using SharedKernel.Domain;

namespace Domain.Products;
public sealed class Category : BaseAuditableEntity
{
	public string Name { get; private set; } = null!;
	public string? Description { get; private set; }
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

	private Category() { }
}
