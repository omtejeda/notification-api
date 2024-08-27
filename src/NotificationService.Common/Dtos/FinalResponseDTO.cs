using static NotificationService.Common.Utils.ResponseUtil;

namespace NotificationService.Common.Dtos
{
    public class FinalResponseDto<T> where T : class
    {
        public FinalResponseDto(int code, T data = null)
        {
            Response = GetResponseObject(code);
            Data = data;
        }

        public FinalResponseDto(int code, T data, PaginationDto pagination)
        {
            Response = GetResponseObject(code);
            Data = data;
            Pagination = pagination;
        }

        public FinalResponseDto(int code, string message, T data = null)
        {
            Response = GetResponseObject(code, message);
            Data = data;
        }

        public ResponseDto Response { get; private set; }
        public PaginationDto Pagination { get; set; }
        public T Data { get; private set; }
    }
}