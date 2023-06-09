using System;
using System.Linq;
using System.Collections.Generic;
using NotificationService.Enums;

namespace NotificationService.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<object> GetCodesAndItsDescription(int? code = null)
        {
            var filterByCode = code.HasValue ? true : false;

            var codes = ( (ErrorCode[]) Enum.GetValues(typeof(ErrorCode)))
                    .Where(x => filterByCode ? (int) x == (int) code : true )
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
}