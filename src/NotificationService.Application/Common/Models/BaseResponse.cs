using NotificationService.Application.Common.Dtos;
using NotificationService.Domain.Enums;

namespace NotificationService.Application.Common.Models;

/// <summary>
/// Represents a standard response format for API responses, 
/// encapsulating the response metadata, data, and pagination information.
/// </summary>
/// <typeparam name="T">The type of the data being returned in the response.</typeparam>
public class BaseResponse<T> where T : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseResponse{T}"/> class
    /// with the specified response code and optional data.
    /// </summary>
    /// <param name="code">The result code indicating the status of the request.</param>
    /// <param name="data">The data to include in the response (optional).</param>
    public BaseResponse(int code, T? data = null)
    {
        Response = ResponseDto.CreateResponse(code);
        Data = data;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseResponse{T}"/> class
    /// with the specified response code, data, and pagination information.
    /// </summary>
    /// <param name="code">The result code indicating the status of the request.</param>
    /// <param name="data">The data to include in the response.</param>
    /// <param name="pagination">The pagination information for the response.</param>
    public BaseResponse(int code, T data, PaginationDto pagination)
    {
        Response = ResponseDto.CreateResponse(code);
        Data = data;
        Pagination = pagination;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseResponse{T}"/> class
    /// with the specified response code, message, and optional data.
    /// </summary>
    /// <param name="code">The result code indicating the status of the request.</param>
    /// <param name="message">A message providing additional information about the response.</param>
    /// <param name="data">The data to include in the response (optional).</param>
    public BaseResponse(int code, string message, T? data = null)
    {
        Response = ResponseDto.CreateResponse(code, message);
        Data = data;
    }

    /// <summary>
    /// Gets the response metadata, including the response code and message.
    /// </summary>
    public ResponseDto Response { get; private set; }

    /// <summary>
    /// Gets or sets the pagination information associated with the response.
    /// </summary>
    public PaginationDto? Pagination { get; set; }

    /// <summary>
    /// Gets the data included in the response.
    /// </summary>
    public T? Data { get; private set; }

    /// <summary>
    /// Creates a successful response with the specified data.
    /// </summary>
    /// <param name="data">The data to include in the successful response.</param>
    /// <returns>A <see cref="BaseResponse{T}"/> representing a successful response.</returns>
    public static BaseResponse<T> Success(T data)
    {
        int successCode = (int)ResultCode.OK;
        return new(successCode, data);
    }

    /// <summary>
    /// Creates a successful response with the specified data and pagination information.
    /// </summary>
    /// <param name="data">The data to include in the successful response.</param>
    /// <param name="pagination">The pagination information for the response.</param>
    /// <returns>A <see cref="BaseResponse{T}"/> representing a successful response.</returns>
    public static BaseResponse<T> Success(T data, PaginationDto pagination)
    {
        int successCode = (int)ResultCode.OK;
        return new(successCode, data, pagination);
    }
}