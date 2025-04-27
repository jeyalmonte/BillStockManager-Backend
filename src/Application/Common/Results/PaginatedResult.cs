using Microsoft.EntityFrameworkCore;

namespace Application.Common.Results;

public readonly record struct PaginatedResult<T>
{
    private PaginatedResult(IReadOnlyCollection<T> items, int totalItems, int pageNumber, int pageSize)
    {
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        TotalItems = totalItems;
        Items = items;
    }

    public IReadOnlyCollection<T> Items { get; }
    public int CurrentPage { get; }
    public int CurrentPageCount => Items.Count;
    public int TotalItems { get; }
    public int TotalPages { get; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    public static PaginatedResult<T> Create(IReadOnlyCollection<T> items, int totalItems, int pageNumber, int pageSize)
        => new(items, totalItems, pageNumber, pageSize);

    public static async Task<PaginatedResult<TSource>> CreateAsync<TSource>(IQueryable<TSource> source, int pageNumber, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var total = await source
            .CountAsync(cancellationToken);

        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<TSource>(items, total, pageNumber, pageSize);
    }
}