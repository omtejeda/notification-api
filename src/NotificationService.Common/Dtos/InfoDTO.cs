using System;
using static NotificationService.Common.Utils.SystemUtil;
namespace NotificationService.Common.Dtos
{
    public class InfoDTO
    {
        public bool IsProduction => IsProduction();
        public string Environment => GetEnvironment();
        public DateTime SystemDate => GetSystemDate();
        public int? GMT => GetGMT();
        public int? LimitPageSize => GetLimitPageSize();
    }
}