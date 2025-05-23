﻿using Domain.Billing;
using Domain.Inventory;
using FluentAssertions;
using SharedKernel.Results;

namespace Domain.UnitTests.Billing;
public class InvoiceDetailTests
{
	[Fact]
	public void Create_WhenQuantityIsLessThanOrEqualToZero_ShouldReturnConflict()
	{
		// Arrange
		var invoiceId = Guid.NewGuid();
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		var result = InvoiceDetail.Create(
			invoiceId: invoiceId,
			product: product,
			quantity: 0
		);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void Create_WhenUnitPriceIsLessThanOrEqualToZero_ShouldReturnConflict()
	{
		// Arrange
		var invoiceId = Guid.NewGuid();
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 0,
			stock: 10
		);

		// Act
		var result = InvoiceDetail.Create(
			invoiceId: invoiceId,
			product: product,
			quantity: 1
		);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void Create_WhenDiscountIsNegative_ShouldReturnConflict()
	{
		// Arrange
		var invoiceId = Guid.NewGuid();
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		var result = InvoiceDetail.Create(
			invoiceId: invoiceId,
			product: product,
			quantity: 1,
			discount: -1
		);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.Single().ErrorType.Should().Be(ErrorType.Conflict);
	}

	[Fact]
	public void Create_ShouldReturnAnInvoiceDetailInstance()
	{
		// Arrange
		var invoiceId = Guid.NewGuid();
		var product = Product.Create(
			name: "Test",
			categoryId: Guid.NewGuid(),
			description: null,
			price: 100,
			stock: 10
		);

		// Act
		var result = InvoiceDetail.Create(
			invoiceId: invoiceId,
			product: product,
			quantity: 1
		);

		// Assert
		result.HasError.Should().BeFalse();
		result.Value.Should().NotBeNull();
		result.Value.Should().BeOfType<InvoiceDetail>();
	}
}
