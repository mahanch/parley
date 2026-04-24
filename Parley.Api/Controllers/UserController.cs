using MediatR;
using Microsoft.AspNetCore.Mvc;
using Parley.Application._Shared.DTOs;
using Parley.Application.Features.Users.Commands.CreateUser;
using Parley.Application.Features.Users.Queries;
using Parley.Domain.Aggregates.UserAgg.Entities;

namespace Parley.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse>> CreateUser([FromBody] CreateUserCommand command)
    {
        var res =await _mediator.Send(command);
        return Ok(res);
    }

    /// <summary>
    /// Authenticates a user.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(BaseResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<LoginResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginQuery query)
    {
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Gets a user's profile.
    /// </summary>
    [HttpGet("profile/{userId}")]
    [ProducesResponseType(typeof(BaseResponse<UserProfileResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<UserProfileResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile(Guid userId)
    {
        var query = new GetUserProfileQuery { UserId = userId };
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}