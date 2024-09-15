namespace NotificationService.Domain.Models;

public class Pagination
{
    public Pagination(int? page, int? pageSize, int pageCount, int totalCount)
    {
        Page = page ?? 1;
        PageSize = pageSize ?? totalCount;
        PageCount = pageCount;

        TotalPages = (int) Math.Ceiling( (decimal) totalCount /  (int) (PageSize == Decimal.Zero ? ++PageSize : PageSize));
        TotalCount = totalCount;
    }

    public Pagination(int page, int pageSize, int totalPages, int totalCount)
    {
        Page = page;
        PageSize = pageSize;
        TotalPages = totalPages;
        TotalCount = totalCount;
    }
    
    public int? Page { get; private set; }
    public int? PageSize { get; private set; }
    public int? TotalPages { get; private set; }
    public int? PageCount { get; private set; }
    public int? TotalCount { get; private set; }
}