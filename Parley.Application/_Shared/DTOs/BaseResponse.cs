namespace Parley.Application._Shared.DTOs;

/// <summary>
/// Base DTO for command/query responses.
/// </summary>
public class BaseResponse
{
    /// <summary>
    /// Indicates if the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Message describing the result (success or error).
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Optional error details.
    /// </summary>
    public Dictionary<string, string[]>? Errors { get; set; }
    
    
    public ErrorType ErrorType { get; set; } = ErrorType.None;

    /// <summary>
    /// Creates a successful response.
    /// </summary>
    public static BaseResponse Success(string message = "Operation completed successfully")
    {
        return new BaseResponse
        {
            IsSuccess = true,
            Message = message
        };
    }

    /// <summary>
    /// Creates a failed response with a single error.
    /// </summary>
    public static BaseResponse Failure(string message, ErrorType errorType = ErrorType.BadRequest,
        string? errorKey = null, params string[] errorValues)
    {
        var response = new BaseResponse
        {
            IsSuccess = false,
            Message = message,
            ErrorType = errorType,
            Errors = new Dictionary<string, string[]>()
        };

        if (!string.IsNullOrEmpty(errorKey))
            response.Errors[errorKey] = errorValues;

        return response;
    }

    /// <summary>
    /// Creates a failed response with multiple errors.
    /// </summary>
    public static BaseResponse Failure(string message, ErrorType errorType, Dictionary<string, string[]> errors) =>
        new() { IsSuccess = false, Message = message, ErrorType = errorType, Errors = errors };
    
    
    public static BaseResponse NotFound(string message) =>
        new() { IsSuccess = false, Message = message, ErrorType = ErrorType.NotFound };

    public static BaseResponse Unauthorized(string message) =>
        new() { IsSuccess = false, Message = message, ErrorType = ErrorType.Unauthorized };

    public static BaseResponse ValidationError(string message) =>
        new() { IsSuccess = false, Message = message, ErrorType = ErrorType.Validation };

    public static BaseResponse ValidationError(Dictionary<string, string[]> errors) =>
        new() { IsSuccess = false, Message = "Validation failed", ErrorType = ErrorType.Validation, Errors = errors };

    public static BaseResponse InternalServerError(string message) =>
        new() { IsSuccess = false, Message = message, ErrorType = ErrorType.InternalServerError };
    
}

/// <summary>
/// Generic base response DTO with data payload.
/// </summary>
public class BaseResponse<T> : BaseResponse
{
    /// <summary>
    /// The response data payload.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Creates a successful response with data.
    /// </summary>
    public static BaseResponse<T> Success(T data, string message = "Operation completed successfully")
    {
        return new BaseResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// Creates a failed response with a single error.
    /// </summary>
    public new static BaseResponse<T> Failure(string message, ErrorType errorType = ErrorType.BadRequest,
        string? errorKey = null, params string[] errorValues)
    {
        var response = new BaseResponse<T>
        {
            IsSuccess = false,
            Message = message,
            ErrorType = errorType,
            Errors = new Dictionary<string, string[]>()
        };

        if (!string.IsNullOrEmpty(errorKey))
            response.Errors[errorKey] = errorValues;

        return response;
    }

    /// <summary>
    /// Creates a failed response with multiple errors.
    /// </summary>
    public static new BaseResponse<T> Failure(string message, ErrorType errorType, Dictionary<string, string[]> errors) =>
        new() { IsSuccess = false, Message = message, ErrorType = errorType, Errors = errors };
}