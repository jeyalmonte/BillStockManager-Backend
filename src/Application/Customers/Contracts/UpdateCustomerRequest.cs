namespace Application.Customers.Contracts;
public record UpdateCustomerRequest(
	string FullName,
	string? Nickname,
	string Gender,
	string? Email,
	string? PhoneNumber,
	string? Address
	);

