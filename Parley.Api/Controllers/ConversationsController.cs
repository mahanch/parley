using MediatR;
using Microsoft.AspNetCore.Mvc;
using Parley.Application._Shared.DTOs;
using Parley.Application.Features.Conversations.Commands;
using Parley.Application.Features.Conversations.Queries;

namespace Parley.Api.Controllers;

/// <summary>
/// Controller for managing conversations (direct messages and group chats).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ConversationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConversationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new direct conversation between two users.
    /// If a direct conversation already exists, returns the existing one.
    /// </summary>
    [HttpPost("direct")]
    [ProducesResponseType(typeof(BaseResponse<CreateConversationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<CreateConversationResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDirectConversation([FromBody] CreateDirectConversationCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Creates a new group conversation.
    /// </summary>
    [HttpPost("group")]
    [ProducesResponseType(typeof(BaseResponse<CreateConversationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<CreateConversationResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGroupConversation([FromBody] CreateGroupConversationCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Gets all conversations for a user.
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(BaseResponse<GetUserConversationsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserConversations(Guid userId)
    {
        var query = new GetUserConversationsQuery { UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}