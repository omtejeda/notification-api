namespace NotificationService.Application.Utils;

public record FilterOptions
{
    private const int MinPage = 1;
    private const int LimitPageSize = 50;

    public int Page { get; init; }
    public int PageSize { get; init; }
    public string Sort { get; init; }
    public IReadOnlyList<string> SortFields => SortHelper.GetSortFields(Sort);
    
    public FilterOptions(int page, int pageSize, string? sort )
    {
        Page = Math.Max(page, MinPage);
        PageSize = Math.Min(pageSize, LimitPageSize);
        Sort = string.IsNullOrWhiteSpace(sort) ? string.Empty : sort;
    }
}