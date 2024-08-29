using System;
using NotificationService.Common.Enums;
using NotificationService.Common.Dtos;
namespace NotificationService.Common.Utils
{
    public static class ResponseUtil
    {
        public static ResponseDto GetResponseObject(int code)
        {
            var message = ((ResultCode) code).ToString();
            return GetResponseObject(code, message);
        }

        public static ResponseDto GetResponseObject(int code, string message)
        {
            return new ResponseDto
            {
                Success = IsSuccess(code),
                Code = code,
                Message = message,
                Date = SystemUtil.GetSystemDate()
            };
        }

        public static ResponseDto GetResponseObject(ResultCode resultCode)
        {
            var message = resultCode.ToString();
            return GetResponseObject(resultCode, message);
        }

        public static ResponseDto GetResponseObject(ResultCode resultCode, string message)
        {
            int code = (int) resultCode;
            return new ResponseDto
            {
                Success = IsSuccess(code),
                Code = code,
                Message = message,
                Date = SystemUtil.GetSystemDate()
            };
        }

        public static bool IsSuccess(int code)
        {
            return code % 10 == 0;
        }
    }
}