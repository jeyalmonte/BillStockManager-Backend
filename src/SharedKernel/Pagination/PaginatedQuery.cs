namespace SharedKernel.Pagination;

public abstract record PaginatedQuery
{
	private const int DefaultPageNumber = 1;
	private const int DefaultPageSize = 20;
	private readonly int? _pageSize;
	private readonly int? _pageNumber;

	public int PageNumber
	{
		get => _pageNumber ?? DefaultPageNumber;
		init => _pageNumber = value;
	}
	public int PageSize
	{
		get => _pageSize ?? DefaultPageSize;
		init => _pageSize = value;
	}
}