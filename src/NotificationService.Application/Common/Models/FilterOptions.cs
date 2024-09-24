using NotificationService.Application.Utils;

namespace NotificationService.Application.Common.Models;

public record FilterOptions
{
    private const int MinPage = 1;
    private const int MaxPageSize = 50;
    private IReadOnlyList<string>? _sortFields;

    public int Page { get; init; } = MinPage;
    public int PageSize { get; init; } = MaxPageSize;
    public string Sort { get; init; } = string.Empty;
    public IReadOnlyList<string> SortFields => _sortFields ??= SortHelper.GetSortFields(Sort);
    public bool HasSortFields => SortFields.Count > 0;
    
    private FilterOptions() {}
    public FilterOptions(int? page, int? pageSize, string? sort = null)
    {
        Page = page > 0
        ? page.Value
        : MinPage;

        PageSize = pageSize.HasValue
        ? Math.Clamp(pageSize.Value, 1, MaxPageSize)
        : MaxPageSize;

        Sort = string.IsNullOrWhiteSpace(sort)
        ? string.Empty
        : sort;
    }

    public static FilterOptions Default() => new();
}