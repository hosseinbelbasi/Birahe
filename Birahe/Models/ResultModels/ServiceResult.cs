using Birahe.EndPoint.Enums;

namespace Birahe.EndPoint.Models.ResultModels;



public class ServiceResult<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public ErrorType Error { get; set; } = ErrorType.None;

    public static ServiceResult<T> Ok(T data, string? message = null, bool success = true) =>
        new ServiceResult<T> { Success = success, Data = data, Message = message };

    public static ServiceResult<T> Fail(string message, ErrorType error = ErrorType.Validation) =>
        new ServiceResult<T> { Success = false, Message = message, Error = error };
}
