using MongoDB.Driver;
namespace NotificationService.Infrastructure.Repositories.Helpers;

public static class RepositoryHelper
{
    public static IFindFluent<TSource, TSource> Paginate<TSource>(this IFindFluent<TSource, TSource> source, int? page, int? pageSize)
    {
        page ??= 1;
        return source.Skip((page - 1) * pageSize).Limit(pageSize);
    }
}