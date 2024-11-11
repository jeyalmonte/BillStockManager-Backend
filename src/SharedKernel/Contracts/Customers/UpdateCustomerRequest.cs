namespace SharedKernel.Contracts.Customers;
public record UpdateCustomerRequest(
	string FullName,
	string? Nickname,
	string Gender,
	string? Email,
	string? PhoneNumber,
	string? Address
	);

