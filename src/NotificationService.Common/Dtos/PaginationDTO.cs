namespace NotificationService.Common.Dtos
{
    public class PaginationDTO
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? PageCount { get; set; }
        public int? TotalPages { get; set; }
        public int? TotalCount { get; set; }
    }
}