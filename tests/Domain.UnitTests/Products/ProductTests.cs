﻿using Domain.Products;
using FluentAssertions;
using SharedKernel.Results;

namespace Domain.UnitTests.Products;
public class ProductTests
{
	[Fact]
	public void Create_ShouldReturnProductInstance()
	{
		// Act
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Assert
		product.Should().NotBeNull();
		product.Should().BeOfType<Product>();
	}

	[Fact]
	public void Update_ShouldReturnConflict_WhenStockIsLessThanZero()
	{
		// Arrange
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		var result = product.Update(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			newStock: -1
		);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void Update_ShouldReturnSuccess()
	{
		// Arrange
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		var result = product.Update(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			newStock: 20
		);

		// Assert
		result.HasError.Should().BeFalse();
		product.Stock.Should().Be(20);
	}

	[Fact]
	public void Remove_ShouldMarkProductAsDeleted()
	{
		// Arrange
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		product.Remove();

		// Assert
		product.IsDeleted.Should().BeTrue();
	}

	[Fact]
	public void ReduceStock_ShouldReturnConflict_WhenQuantityIsLessThanOrEqualToZero()
	{
		// Arrange
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		var result = product.ReduceStock(quantity: 0);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void ReduceStock_ShouldReturnConflict_WhenQuantityExceedsAvailableStock()
	{
		// Arrange
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		var result = product.ReduceStock(quantity: 20);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void ReduceStock_ShouldReturnSuccess()
	{
		// Arrange
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		var result = product.ReduceStock(quantity: 5);

		// Assert
		result.HasError.Should().BeFalse();
		product.Stock.Should().Be(5);
	}

	[Fact]
	public void IncreaseStock_ShouldReturnConflict_WhenQuantityIsLessThanOrEqualToZero()
	{
		// Arrange
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		var result = product.IncreaseStock(quantity: 0);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void IncreaseStock_ShouldReturnSuccess()
	{
		// Arrange
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		var result = product.IncreaseStock(quantity: 5);

		// Assert
		result.HasError.Should().BeFalse();
		product.Stock.Should().Be(15);
	}
}
