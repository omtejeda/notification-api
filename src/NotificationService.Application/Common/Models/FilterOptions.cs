using NotificationService.Application.Common.Helpers;

namespace NotificationService.Application.Common.Models;

/// <summary>
/// Represents the options for filtering, pagination, and sorting data in API requests.
/// </summary>
public record FilterOptions
{
    private const int MinPage = 1;
    private const int MaxPageSize = 50;
    private IReadOnlyList<string>? _sortFields;

    /// <summary>Gets the current page number. Defaults to the minimum page.</summary>
    public int Page { get; init; } = MinPage;

    /// <summary>Gets the number of items to return per page. Defaults to the maximum page size.</summary>
    public int PageSize { get; init; } = MaxPageSize;

    /// <summary>Gets the sorting criteria as a string. Defaults to an empty string.</summary>
    public string Sort { get; init; } = string.Empty;

    /// <summary>Gets the parsed list of sort fields based on the Sort property.</summary>
    public IReadOnlyList<string> SortFields => _sortFields ??= SortHelper.GetSortFields(Sort);

    /// <summary>Indicates whether there are any sort fields specified.</summary>
    public bool HasSortFields => SortFields.Count > 0;

    // Private constructor for internal use.
    private FilterOptions() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="FilterOptions"/> record
    /// with the specified page number, page size, and sort criteria.
    /// </summary>
    /// <param name="page">The requested page number.</param>
    /// <param name="pageSize">The number of items to return per page.</param>
    /// <param name="sort">The sorting criteria as a string (optional).</param>
    public FilterOptions(int? page, int? pageSize, string? sort = null)
    {
        Page = page > 0 ? page.Value : MinPage;

        PageSize = pageSize.HasValue
            ? Math.Clamp(pageSize.Value, 1, MaxPageSize)
            : MaxPageSize;

        Sort = string.IsNullOrWhiteSpace(sort) ? string.Empty : sort;
    }

    /// <summary>
    /// Creates a default instance of the <see cref="FilterOptions"/> record
    /// </summary>
    /// <returns>A <see cref="FilterOptions"/> record initialized with default values.</returns>
    public static FilterOptions Default() => new();
}