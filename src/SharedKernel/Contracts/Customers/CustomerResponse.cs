namespace SharedKernel.Contracts.Customers;
public record CustomerResponse(
    Guid Id,
    string FullName,
    string? Nickname,
    string DocumentType,
    string Document,
    string Gender,
    string? Email,
    string? PhoneNumber,
    string? Address
    );

