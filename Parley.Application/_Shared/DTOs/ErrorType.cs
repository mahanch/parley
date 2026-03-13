namespace Parley.Application._Shared.DTOs;

public enum ErrorType
{
    None,
    BadRequest,
    NotFound,
    Unauthorized,
    Validation,
    InternalServerError
}