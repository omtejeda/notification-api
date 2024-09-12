using System;
using NotificationService.Domain.Enums;
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

        public static ResponseDto CreateResponse(int code, string? message = null)
        {
            var resultMessage 
                = message ?? ((ResultCode) code).ToString();
            
            return new()
            {
                Success = IsSuccess(code),
                Code = code,
                Message = resultMessage,
                Date = AppUtil.CurrentDate
            };
        }
        
        private static bool IsSuccess(int code)
            => code % 10 == 0;
    }
}