using static NotificationService.Common.Utils.ResponseUtil;

namespace NotificationService.Common.Dtos
{
    public class FinalResponseDTO<T> where T : class
    {
        public FinalResponseDTO(int code, T data = null)
        {
            Response = GetResponseObject(code);
            Data = data;
        }

        public FinalResponseDTO(int code, T data, PaginationDTO pagination)
        {
            Response = GetResponseObject(code);
            Data = data;
            Pagination = pagination;
        }

        public FinalResponseDTO(int code, string message, T data = null)
        {
            Response = GetResponseObject(code, message);
            Data = data;
        }

        public ResponseDTO Response { get; private set; }
        public PaginationDTO Pagination { get; set; }
        public T Data { get; private set; }
    }
}