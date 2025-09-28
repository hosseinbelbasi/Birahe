namespace Birahe.EndPoint.Enums;

public enum ErrorType
{
    None,
    Validation,   // BadRequest (400)
    NotFound,     // 404
    ServerError   // 500
}