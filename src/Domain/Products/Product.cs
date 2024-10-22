using Domain.Products.Events;
using SharedKernel.Domain;
using SharedKernel.Results;

namespace Domain.Products;
public sealed class Product : BaseAuditableEntity
{
	public string Name { get; private set; } = null!;
	public Guid CategoryId { get; private set; }
	public Category Category { get; private set; } = null!;
	public string? Description { get; private set; }
	public decimal Price { get; private set; }
	public int Stock { get; private set; }
	public decimal? Discount { get; private set; }
	public Product(string name, Guid categoryId, string? description, decimal price, int stock, decimal? discount)
	{
		Name = name;
		CategoryId = categoryId;
		Description = description;
		Price = price;
		Stock = stock;
		Discount = discount;
	}

	public static Product Create(string name, Guid categoryId, string? description, decimal price, int stock, decimal? discount)
	{
		var product = new Product(
			name: name,
			categoryId: categoryId,
			description: description,
			price: price,
			stock: stock,
			discount: discount);

		product.RaiseEvent(new ProductCreatedEvent(product));
		return product;
	}

	public Result<Success> ReduceStock(int quantity)
	{
		if (quantity <= 0)
		{
			return Error.Failure(description: "Quantity must be greater than zero.");
		}

		if (Stock < quantity)
		{
			return Error.Failure(description: "Quantity exceeds the available stock.");
		}

		Stock -= quantity;

		RaiseEvent(new ProductStockReducedEvent(this, quantity));

		return Result.Success;
	}

	public Result<Success> IncreaseStock(int quantity)
	{
		if (quantity <= 0)
		{
			return Error.Failure(description: "Quantity must be greater than zero.");
		}

		Stock += quantity;

		RaiseEvent(new ProductStockIncreasedEvent(this, quantity));

		return Result.Success;
	}

	public decimal GetPriceAfterDiscount()
	{
		if (!Discount.HasValue || Discount.Value <= 0)
		{
			return Price;
		}

		return Price - Discount.Value;
	}

	private Product() { }
}
