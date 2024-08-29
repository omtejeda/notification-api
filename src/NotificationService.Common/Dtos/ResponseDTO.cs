using System;
using NotificationService.Common.Enums;
using NotificationService.Common.Utils;
namespace NotificationService.Common.Dtos
{
    public class ResponseDto
    {
        public bool? Success { get; set; }
        public int? Code { get; set; }
        public string? Message { get; set; }
        public DateTime? Date { get; set; }

        private ResponseDto() {}

        public static ResponseDto CreateResponse(int code) 
            => GetInstance(code, message: ((ResultCode) code).ToString());

        public static ResponseDto CreateResponse(int code, string message)
            => GetInstance(code, message);

        private static ResponseDto GetInstance(int code, string? message)
            => new()
            {
                Success = IsSuccess(code),
                Code = code,
                Message = message ?? string.Empty,
                Date = SystemUtil.GetSystemDate()
            };
        
        private static bool IsSuccess(int code)
            => code % 10 == 0;
    }
}