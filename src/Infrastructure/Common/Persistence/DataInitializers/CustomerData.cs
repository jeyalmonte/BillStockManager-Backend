using Domain.Customers;

namespace Infrastructure.Common.Persistence.DataInitializers;
internal static class CustomerData
{
	public static List<Customer> GetData()
		=>
		[
			new("John Doe", "JD",GenderType.Male,  "john@gmail.com",  "1234567890",  "123 Main St."),
			new("Jane Doe", "Jane",GenderType.Female,"jane@gmail.com", "0987654321", "456 Elm St."),
			new("Alice Smith", "Alice" ,GenderType.Female, "alice@gmail.com", "1231231234", "789 Oak St."),
			new("Bob Smith", "Bob",GenderType.Male, "bob@gmail.com", "4564564567", "101 Pine St."),
			new("Charlie Brown", "Charlie",GenderType.Male, "charlieb@gmail.com", "7897897890", "1313 Mockingbird Ln."),
			new("Lucy Brown", "Lucy",GenderType.Female, "lucy@gmail.com", "1011011010", "456 Maple St."),
		];
}
