using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parley.Application._Shared.DTOs;
using Parley.Application.Features.Messages.Commands;
using Parley.Application.Features.Messages.Queries;

namespace Parley.Api.Controllers;

/// <summary>
/// Controller for managing messages.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MessagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Sends a new message to a conversation.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<SendMessageResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<SendMessageResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Gets messages for a conversation with pagination.
    /// </summary>
    [HttpGet("conversation/{conversationId}")]
    [ProducesResponseType(typeof(BaseResponse<GetConversationMessagesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<GetConversationMessagesResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetConversationMessages(
        Guid conversationId,
        [FromQuery] int pageSize = 50,
        [FromQuery] long? beforeMessageId = null,
        [FromQuery] Guid requestingUserId = default)
    {
        var query = new GetConversationMessagesQuery
        {
            ConversationId = conversationId,
            PageSize = pageSize,
            BeforeMessageId = beforeMessageId,
            RequestingUserId = requestingUserId
        };

        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
