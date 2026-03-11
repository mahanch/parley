using MediatR;
using Parley.Application._Shared.DTOs;
using Parley.Application.Contracts.Query.Conversation;
using Parley.Application.Features.Messages.Queries;
using Parley.Domain.Aggregates.ConversationAgg;

namespace Parley.Application.Features.Messages.Handlers;

/// <summary>
/// Handler for GetConversationMessagesQuery.
/// Demonstrates:
/// - Cursor-based pagination with Snowflake IDs using IContext
/// - Direct LINQ queries for optimal performance
/// - DTO mapping
/// - Permission verification
/// </summary>
public class GetConversationMessagesQueryHandler : IRequestHandler<GetConversationMessagesQuery, BaseResponse<GetConversationMessagesResponse>>
{

    private readonly IConversationQueryService _conversationQueryService;

    public GetConversationMessagesQueryHandler(IConversationQueryService conversationQueryService)
    {
        _conversationQueryService = conversationQueryService;
    }

    public async Task<BaseResponse<GetConversationMessagesResponse>> Handle(
        GetConversationMessagesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            throw new NotImplementedException();
            var filter = new GetConversationMessagesFilter()
            {
                BeforeMessageId = request.BeforeMessageId,
                PageSize = request.PageSize,
                ConversationId = request.ConversationId,
                RequestingUserId = request.RequestingUserId
            };
            var res =await _conversationQueryService.GetMessagesAsync(filter, cancellationToken);
            
            // Step 8: Build and return response
            var response = new GetConversationMessagesResponse
            {
                // Messages = res.Messages,
                TotalCount = res.TotalCount,
                OldestMessageId = res.OldestMessageId,
                HasMoreMessages = res.HasMoreMessages
            };

            return BaseResponse<GetConversationMessagesResponse>.Success(
                response,
                $"Retrieved {res.Messages?.Count} messages."
            );
        }
        catch (Exception ex)
        {
            // In production, log this exception
            return BaseResponse<GetConversationMessagesResponse>.Failure(
                "An unexpected error occurred while retrieving messages.",
                "internal_error",
                ex.Message
            );
        }
    }
}




