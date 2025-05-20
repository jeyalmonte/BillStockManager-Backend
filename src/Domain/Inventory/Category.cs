using Domain.Inventory.Events;
using SharedKernel.Domain;

namespace Domain.Inventory;
public sealed class Category : AuditableEntity
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

		category.RaiseEvent(new CategoryCreatedDomainEvent(category));

		return category;
	}

	private Category() { }
}
