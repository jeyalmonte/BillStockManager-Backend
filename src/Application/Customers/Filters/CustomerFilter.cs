using Domain.Customers;
using SharedKernel.Enums;

namespace Application.Customers.Filters;
public record CustomerFilter(
	string? FullName,
	string? Nickname,
	DocumentType? DocumentType,
	string? Document,
	GenderType? Gender,
	string? SortBy,
	OrderType OrderBy = OrderType.Ascending
	);
