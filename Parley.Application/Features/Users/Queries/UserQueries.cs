using MediatR;
using Parley.Application._Shared.DTOs;

namespace Parley.Application.Features.Users.Queries;

/// <summary>
/// Query to authenticate a user.
/// </summary>
public class LoginQuery : IRequest<BaseResponse<LoginResponse>>
{
    /// <summary>
    /// Username or email.
    /// </summary>
    public string UsernameOrEmail { get; set; } = string.Empty;

    /// <summary>
    /// Password.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Response for LoginQuery.
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// User ID.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Full name.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// JWT token (if authentication is implemented).
    /// </summary>
    public string? Token { get; set; }
}

/// <summary>
/// Query to get user profile.
/// </summary>
public class GetUserProfileQuery : IRequest<BaseResponse<UserProfileResponse>>
{
    /// <summary>
    /// User ID to get profile for.
    /// </summary>
    public Guid UserId { get; set; }
}

/// <summary>
/// Response for GetUserProfileQuery.
/// </summary>
public class UserProfileResponse
{
    /// <summary>
    /// User ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// First name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Full name.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// When the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
