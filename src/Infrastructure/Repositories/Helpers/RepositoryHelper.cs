using MongoDB.Driver;
namespace NotificationService.Infrastructure.Repositories.Helpers
{
    public static class RepositoryHelper
    {
        public static IFindFluent<TSource, TSource> Paginate<TSource>(this IFindFluent<TSource, TSource> source, int? page, int? pageSize)
        {
            page ??= 1;
            pageSize = GetPageSizeBasedOnLimit(pageSize);
                        
            return source.Skip((page - 1) * pageSize).Limit(pageSize);
        }

        public static int? GetPageSizeBasedOnLimit(int? pageSize)
        {
            var limitPageSize = Core.Common.Utils.SystemUtil.GetLimitPageSize();
            pageSize ??= limitPageSize;
            
            if (limitPageSize == default(int?)) return pageSize;
            if (pageSize > limitPageSize) return limitPageSize;
            
            return pageSize;
        }
    }
}