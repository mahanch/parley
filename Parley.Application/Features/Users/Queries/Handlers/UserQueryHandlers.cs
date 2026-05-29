using MediatR;
using Parley.Application._Shared.DTOs;
using Parley.Application.Contracts.Interfaces.Security;
using Parley.Application.Features.Users.Queries;
using Parley.Domain._Shared;
using Parley.Domain.Aggregates.UserAgg;

namespace Parley.Application.Features.Users.Queries.Handlers;

/// <summary>
/// Handler for LoginQuery.
/// </summary>
public class LoginQueryHandler : IRequestHandler<LoginQuery, BaseResponse<LoginResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthHelper _authHelper;

    public LoginQueryHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IAuthHelper authHelper)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _authHelper = authHelper ?? throw new ArgumentNullException(nameof(authHelper));
    }

    public async Task<BaseResponse<LoginResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Find user by username or email
            var user = await _userRepository.GetByUsernameOrEmailAsync(request.UsernameOrEmail, cancellationToken);

            if (user == null)
            {
                return BaseResponse<LoginResponse>.Failure(
                    "Invalid username/email or password.",
                    ErrorType.BadRequest,
                    "invalid_credentials"
                );
            }

            // Verify password
            var isPasswordValid = _passwordHasher.Verify(request.Password, user.Password);

            if (!isPasswordValid)
            {
                return BaseResponse<LoginResponse>.Failure(
                    "Invalid username/email or password.",
                    ErrorType.BadRequest,
                    "invalid_credentials"
                );
            }

            // Generate JWT Token
            var token = _authHelper.GenerateJwtToken(user.Id, user.Username, user.Email);

            // Return user info
            var response = new LoginResponse
            {
                UserId = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Token = token
            };

            return BaseResponse<LoginResponse>.Success(response, "Login successful.");
        }
        catch (Exception ex)
        {
            return BaseResponse<LoginResponse>.Failure(
                "Login failed.",
                ErrorType.InternalServerError,
                "internal_error",
                ex.Message
            );
        }
    }
}

/// <summary>
/// Handler for GetUserProfileQuery.
/// </summary>
public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, BaseResponse<UserProfileResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetUserProfileQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<BaseResponse<UserProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                return BaseResponse<UserProfileResponse>.Failure(
                    "User not found.",
                    ErrorType.NotFound,
                    "user_not_found"
                );
            }

            var response = new UserProfileResponse
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };

            return BaseResponse<UserProfileResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return BaseResponse<UserProfileResponse>.Failure(
                "Failed to retrieve user profile.",
                ErrorType.InternalServerError,
                "internal_error",
                ex.Message
            );
        }
    }
}
