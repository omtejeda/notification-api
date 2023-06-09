using System;
using NotificationService.Enums;
using NotificationService.Dtos;
namespace NotificationService.Utils
{
    public static class ResponseUtil
    {
        public static ResponseDTO GetResponseObject(int code)
        {
            var message = ((ErrorCode) code).ToString();
            return GetResponseObject(code, message);
        }

        public static ResponseDTO GetResponseObject(int code, string message)
        {
            return new ResponseDTO
            {
                Success = IsSuccess(code),
                Code = code,
                Message = message,
                Date = SystemUtil.GetSystemDate()
            };
        }

        public static ResponseDTO GetResponseObject(ErrorCode errorCode)
        {
            var message = errorCode.ToString();
            return GetResponseObject(errorCode, message);
        }

        public static ResponseDTO GetResponseObject(ErrorCode errorCode, string message)
        {
            int code = (int) errorCode;
            return new ResponseDTO
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