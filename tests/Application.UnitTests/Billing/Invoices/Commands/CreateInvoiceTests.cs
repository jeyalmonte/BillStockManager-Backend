using Application.Billing.Invoices.Commands.Create;
using Application.Billing.Invoices.Services.Interfaces;
using Domain.Billing;
using Domain.Billing.Repositories;
using Domain.Customers;
using Domain.Customers.Repositories;
using Domain.Inventory;
using Domain.Inventory.Repositories;
using SharedKernel.Contracts.Invoices;
using SharedKernel.Interfaces;

namespace Application.UnitTests.Billing.Invoices.Commands;
public class CreateInvoiceTests
{
	private readonly Mock<IInvoiceService> _invoiceService = new();
	private readonly Mock<IInvoiceRepository> _invoiceRepository = new();
	private readonly Mock<IProductRepository> _productRepository = new();
	private readonly Mock<ICustomerRepository> _customerRepository = new();
	private readonly Mock<IUnitOfWork> _unitOfWork = new();

	[Fact]
	public void CreateInvoice_WhenCustomerIdIsEmpty_ShouldReturnFalse()
	{
		// Arrange
		var command = GetCreateInvoiceCommand(hasCustomerId: false);
		var validator = new CreateInvoiceCommandValidator();

		// Act
		var result = validator.Validate(command);

		// Assert
		result.IsValid.Should().BeFalse();
	}

	[Fact]
	public async Task CreateInvoice_WhenCustomerNotFound_ShouldReturnFailureError()
	{
		// Arrange
		var command = GetCreateInvoiceCommand();
		_customerRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((Customer?)null);

		var handler = new CreateInvoiceCommandHandler(
			_invoiceService.Object,
			_invoiceRepository.Object,
			_productRepository.Object,
			_customerRepository.Object,
			_unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Failure);
	}

	[Fact]
	public async Task CreateInvoice_WhenSomeProductsNotFound_ShouldReturnFailureError()
	{
		// Arrange
		var command = GetCreateInvoiceCommand();
		var customer = GetCustomer();

		_customerRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(customer);

		_productRepository.Setup(x => x.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync([]);

		var handler = new CreateInvoiceCommandHandler(
			_invoiceService.Object,
			_invoiceRepository.Object,
			_productRepository.Object,
			_customerRepository.Object,
			_unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Failure);
	}

	[Fact]
	public async Task CreateInvoice_WhenAddInvoiceDetailsFails_ShouldReturnFailureError()
	{
		// Arrange
		var command = GetCreateInvoiceCommand();
		var customer = GetCustomer();
		var product = GetProduct();

		_customerRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(customer);

		_productRepository.Setup(x => x.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<Product> { product });

		_invoiceService.Setup(x => x.AddInvoiceDetails(It.IsAny<Invoice>(), It.IsAny<List<Product>>(), It.IsAny<List<CreateInvoiceDetailRequest>>()))
			.Returns(Error.Failure(description: "Error"));

		var handler = new CreateInvoiceCommandHandler(
			_invoiceService.Object,
			_invoiceRepository.Object,
			_productRepository.Object,
			_customerRepository.Object,
			_unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeTrue();
		result.Errors.First().ErrorType.Should().Be(ErrorType.Failure);
	}

	[Fact]
	public async Task CreateInvoice_WhenCommandIsValid_ShouldCreateInvoice()
	{
		// Arrange
		var command = GetCreateInvoiceCommand();
		var customer = GetCustomer();
		var product = GetProduct();

		_customerRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(customer);

		_productRepository.Setup(x => x.GetByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<Product> { product });

		_invoiceService.Setup(x => x.AddInvoiceDetails(It.IsAny<Invoice>(), It.IsAny<List<Product>>(), It.IsAny<List<CreateInvoiceDetailRequest>>()))
			.Returns(Result.Success);

		_invoiceRepository.Setup(x => x.Add(It.IsAny<Invoice>()));

		var handler = new CreateInvoiceCommandHandler(
			_invoiceService.Object,
			_invoiceRepository.Object,
			_productRepository.Object,
			_customerRepository.Object,
			_unitOfWork.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.HasError.Should().BeFalse();
		result.Value.Should().NotBeNull();
		result.Value.Should().BeOfType<InvoiceResponse>();

		_invoiceRepository.Verify(x => x.Add(It.IsAny<Invoice>()), Times.Once);
		_unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
	}

	private static CreateInvoiceCommand GetCreateInvoiceCommand(bool hasCustomerId = true)
		=> new(
			CustomerId: hasCustomerId ? Guid.NewGuid() : default!,
			InvoiceDetails:
			[
				new CreateInvoiceDetailRequest(
					ProductId: Guid.NewGuid(),
					Quantity: 1,
					Discount: 0
					)
			]);

	private static Customer GetCustomer()
		=> Customer.NewBuilder()
			.WithFullName("test")
			.WithNickname("test")
			.WithDocumentType(DocumentType.Cedula)
			.WithDocument("12345678901")
			.WithGender(GenderType.Male)
			.WithEmail("test@test.com")
			.WithPhoneNumber("1234567890")
			.WithAddress("test")
			.Build();

	private static Product GetProduct()
		=> Product.Create(
			name: "test",
			categoryId: Guid.NewGuid(),
			description: "test",
			price: 1,
			stock: 1,
			discount: 0);
}
