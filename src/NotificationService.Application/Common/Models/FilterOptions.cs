using NotificationService.Application.Utils;

namespace NotificationService.Application.Common.Models;

public record FilterOptions
{
    private const int MinPage = 1;
    private const int LimitPageSize = 50;
    private IReadOnlyList<string>? _sortFields;

    public int Page { get; init; } = MinPage;
    public int PageSize { get; init; } = LimitPageSize;
    public string Sort { get; init; } = string.Empty;
    public IReadOnlyList<string> SortFields => _sortFields ??= SortHelper.GetSortFields(Sort);
    
    private FilterOptions() {}
    public FilterOptions(int? page, int? pageSize, string? sort = null)
    {
        Page = Math.Max(page ?? MinPage, MinPage);
        PageSize = Math.Min(pageSize ?? LimitPageSize, LimitPageSize);
        Sort = string.IsNullOrWhiteSpace(sort) ? string.Empty : sort;
    }

    public static FilterOptions Default() => new();
}