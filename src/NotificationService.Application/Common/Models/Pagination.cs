namespace NotificationService.Application.Common.Models;

/// <summary>
/// Represents pagination details for a collection of data.
/// </summary>
public class Pagination
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Pagination"/> class.
    /// </summary>
    /// <param name="page">The current page number (defaults to 1).</param>
    /// <param name="pageSize">The number of items per page (defaults to total count).</param>
    /// <param name="pageCount">The total number of pages.</param>
    /// <param name="totalCount">The total number of items.</param>
    public Pagination(int? page, int? pageSize, int pageCount, int totalCount)
    {
        Page = page ?? 1;
        PageSize = pageSize ?? totalCount;
        PageCount = pageCount;

        TotalPages = (int) Math.Ceiling( (decimal) totalCount /  (int) (PageSize == 0 ? ++PageSize : PageSize));
        TotalCount = totalCount;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Pagination"/> class.
    /// </summary>
    /// <param name="page">The current page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="totalPages">The total number of pages.</param>
    /// <param name="totalCount">The total number of items.</param>
    public Pagination(int page, int pageSize, int totalPages, int totalCount)
    {
        Page = page;
        PageSize = pageSize;
        TotalPages = totalPages;
        TotalCount = totalCount;
    }
    
    /// <summary>Gets the current page number.</summary>
    public int? Page { get; private set; }

    /// <summary>Gets the number of items per page.</summary>
    public int? PageSize { get; private set; }

    /// <summary>Gets the total number of pages calculated based on total count.</summary>
    public int? TotalPages { get; private set; }

    /// <summary>Gets the total number of items available in the current page.</summary>
    public int? PageCount { get; private set; }

    /// <summary>Gets the total number of items in the collection.</summary>
    public int? TotalCount { get; private set; }
}