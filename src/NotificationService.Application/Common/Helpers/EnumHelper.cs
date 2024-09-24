using NotificationService.Domain.Enums;

namespace NotificationService.Application.Common.Helpers;

public static class EnumHelper
{
    public static IEnumerable<object> GetCodesAndItsDescription(int? code = null)
    {
        var filterByCode = code.HasValue;

        var codes = ( (ResultCode[]) Enum.GetValues(typeof(ResultCode)))
                .Where(x => !filterByCode || (int)x == (int) Convert.ToInt32(code))
                .Select(c => new  { Code = (int) c, Description = c.ToString() }).ToList();
        
        return codes;
    }

    public static IEnumerable<object> GetProviderTypes(int? code = null)
    {
        var codes = ( (ProviderType[]) Enum.GetValues(typeof(ProviderType)))
                .Select(c => new  { ProviderType = c.ToString() }).ToList();
        
        return codes;
    }
}