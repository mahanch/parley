using MediatR;
using Parley.Application._Shared.DTOs;
using Parley.Application.Contracts.Query.Conversation;
using Parley.Application.Features.Conversations.Queries;

namespace Parley.Application.Features.Conversations.Queries.Handlers;

/// <summary>
/// Handler for GetUserConversationsQuery.
/// </summary>
public class GetUserConversationsQueryHandler : IRequestHandler<GetUserConversationsQuery, BaseResponse<GetUserConversationsResponse>>
{
    private readonly IConversationQueryService _conversationQueryService;

    public GetUserConversationsQueryHandler(IConversationQueryService conversationQueryService)
    {
        _conversationQueryService = conversationQueryService ?? throw new ArgumentNullException(nameof(conversationQueryService));
    }

    public async Task<BaseResponse<GetUserConversationsResponse>> Handle(
        GetUserConversationsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var conversations = await _conversationQueryService.GetUserConversationsAsync(request.UserId, cancellationToken);

            var response = new GetUserConversationsResponse
            {
                Conversations = conversations.Select(c => new ConversationSummaryDto
                {
                    Id = c.ConversationId,
                    Type = "Direct", // TODO: determine type from conversation
                    Name = c.Title,
                    LastMessage = c.LastMessagePreview != null ? new MessageSummaryDto
                    {
                        Id = 0, // TODO: get actual message ID
                        SenderName = "", // TODO: get sender name
                        Text = c.LastMessagePreview,
                        CreatedAt = c.LastMessageTime ?? DateTime.MinValue
                    } : null,
                    UnreadCount = c.UnreadMessagesCount,
                    LastActivityAt = c.LastMessageTime
                }).ToList().AsReadOnly()
            };

            return BaseResponse<GetUserConversationsResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return BaseResponse<GetUserConversationsResponse>.Failure(
                "Failed to retrieve user conversations.",
                ErrorType.InternalServerError,
                "internal_error",
                ex.Message
            );
        }
    }
}
