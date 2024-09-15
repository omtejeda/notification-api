using NotificationService.Domain.Enums;

namespace NotificationService.Common.Dtos;

public class BaseResponse<T> where T : class
{
    public BaseResponse(int code, T? data = null)
    {
        Response = ResponseDto.CreateResponse(code);
        Data = data;
    }

    public BaseResponse(int code, T data, PaginationDto pagination)
    {
        Response = ResponseDto.CreateResponse(code);
        Data = data;
        Pagination = pagination;
    }

    public BaseResponse(int code, string message, T? data = null)
    {
        Response = ResponseDto.CreateResponse(code, message);
        Data = data;
    }

    public ResponseDto Response { get; private set; }
    public PaginationDto? Pagination { get; set; }
    public T? Data { get; private set; }

    public static BaseResponse<T> Success(T data)
    {
        int successCode = (int) ResultCode.OK;
        return new(successCode, data);
    }

    public static BaseResponse<T> Success(T data, PaginationDto pagination)
    {
        int successCode = (int) ResultCode.OK;
        return new(successCode, data, pagination);
    }
}