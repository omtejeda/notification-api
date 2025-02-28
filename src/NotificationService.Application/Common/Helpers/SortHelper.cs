namespace NotificationService.Application.Common.Helpers;

public static class SortHelper
{
    public const char Delimiter = ',';
    
    public static IReadOnlyList<string> GetSortFields(string? sort)
    {
        return string.IsNullOrWhiteSpace(sort) 
        ? []
        : sort.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries);
    }
}