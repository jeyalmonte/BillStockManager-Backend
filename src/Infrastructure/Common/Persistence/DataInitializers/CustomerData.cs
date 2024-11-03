using Domain.Customers;

namespace Infrastructure.Common.Persistence.DataInitializers;
internal static class CustomerData
{
    public static List<Customer> GetData()
        =>
        [
            Create("John Doe", "JD",GenderType.Male,  "john@gmail.com",  "12345678901"),
            Create("Jane Doe", "Jane",GenderType.Female,"jane@gmail.com", "09876543211"),
            Create("Alice Smith", "Alice" ,GenderType.Female, "alice@gmail.com", "12312312343"),
            Create("Bob Smith", "Bob",GenderType.Male, "bob@gmail.com", "45645645673"),
            Create("Charlie Brown", "Charlie",GenderType.Male, "charlieb@gmail.com", "78978978902"),
            Create("Lucy Brown", "Lucy",GenderType.Female, "lucy@gmail.com", "10110110104"),
        ];


    private static Customer Create(string fullname, string nickname, GenderType genderType, string email, string document)
    {
        var customer = Customer.NewBuilder()
            .WithFullName(fullname)
            .WithNickname(nickname)
            .WithDocumentType(DocumentType.Cedula)
            .WithDocument(document)
            .WithGender(genderType)
            .WithEmail(email)
            .WithPhoneNumber("4564564567")
            .WithAddress("1313 Mockingbird Ln.")
            .Build();

        return customer;
    }
}
