using Domain.Inventory.Events;
using SharedKernel.Domain;
using SharedKernel.Results;

namespace Domain.Inventory;
public sealed class Product : AuditableEntity
{
	public string Name { get; private set; } = null!;
	public Guid CategoryId { get; private set; }
	public Category Category { get; private set; } = null!;
	public string? Description { get; private set; }
	public decimal Price { get; private set; }
	public int Stock { get; private set; }
	public decimal? Discount { get; private set; }
	private Product(string name, Guid categoryId, string? description, decimal price, int stock, decimal? discount)
	{
		Name = name;
		CategoryId = categoryId;
		Description = description;
		Price = price;
		Stock = stock;
		Discount = discount;
	}

	public static Product Create(string name, Guid categoryId, string? description, decimal price, int stock, decimal? discount = 0)
	{
		var product = new Product(
			name: name,
			categoryId: categoryId,
			description: description,
			price: price,
			stock: stock,
			discount: discount);

		product.RaiseEvent(new ProductCreatedDomainEvent(product.Id));

		return product;
	}

	public void Update(string name, string? description, Guid categoryId, decimal price, decimal? discount = 0)
	{
		Name = name;
		CategoryId = categoryId;
		Description = description;
		Price = price;
		Discount = discount;

		RaiseEvent(new ProductUpdatedDomainEvent(this));
	}

	public void Remove()
	{
		MarkAsDeleted();
		RaiseEvent(new ProductRemovedDomainEvent(Id));
	}

	public decimal GetPriceAfterDiscount()
	{
		if (!Discount.HasValue || Discount.Value <= 0)
		{
			return Price;
		}

		var totalDiscount = Price * (Discount.Value / 100);

		return Price - totalDiscount;
	}

	public Result<Success> HandleStockChange(int quantity)
	{
		if (quantity == 0)
		{
			return Error.Failure(description: "Quantity cannot be zero.");
		}

		if (quantity > 0)
		{
			return IncreaseStock(quantity);
		}

		return ReduceStock(Math.Abs(quantity));
	}

	private Result<Success> IncreaseStock(int quantity)
	{
		Stock += quantity;

		RaiseEvent(new ProductStockChangedDomainEvent(Id, quantity));

		return Result.Success;
	}

	private Result<Success> ReduceStock(int quantity)
	{
		if (quantity > Stock)
		{
			return Error.Conflict(description: "Quantity exceeds the available stock.");
		}

		Stock -= quantity;

		RaiseEvent(new ProductStockChangedDomainEvent(Id, -quantity));

		return Result.Success;
	}

	private Product() { }
}
