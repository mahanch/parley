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
    public static BaseResponse Failure(string message, string? errorKey = null, params string[] errorValues)
    {
        var response = new BaseResponse
        {
            IsSuccess = false,
            Message = message,
            Errors = new Dictionary<string, string[]>()
        };

        if (!string.IsNullOrEmpty(errorKey))
        {
            response.Errors[errorKey] = errorValues;
        }

        return response;
    }

    /// <summary>
    /// Creates a failed response with multiple errors.
    /// </summary>
    public static BaseResponse Failure(string message, Dictionary<string, string[]> errors)
    {
        return new BaseResponse
        {
            IsSuccess = false,
            Message = message,
            Errors = errors
        };
    }
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
    public static new BaseResponse<T> Failure(string message, string? errorKey = null, params string[] errorValues)
    {
        var response = new BaseResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = new Dictionary<string, string[]>()
        };

        if (!string.IsNullOrEmpty(errorKey))
        {
            response.Errors[errorKey] = errorValues;
        }

        return response;
    }

    /// <summary>
    /// Creates a failed response with multiple errors.
    /// </summary>
    public static new BaseResponse<T> Failure(string message, Dictionary<string, string[]> errors)
    {
        return new BaseResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors
        };
    }
}

