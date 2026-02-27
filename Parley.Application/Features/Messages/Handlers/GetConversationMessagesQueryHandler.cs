using MediatR;
using Microsoft.EntityFrameworkCore;
using Parley.Application._Shared.DTOs;
using Parley.Application.Contracts.Interfaces.Data;
using Parley.Application.Features.Messages.Queries;

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
    private readonly IContext _context;

    public GetConversationMessagesQueryHandler(IContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<BaseResponse<GetConversationMessagesResponse>> Handle(
        GetConversationMessagesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Step 1: Verify conversation exists
            var conversation = await _context.Conversations
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.ConversationId, cancellationToken);

            if (conversation == null)
            {
                return BaseResponse<GetConversationMessagesResponse>.Failure(
                    "Failed to retrieve messages.",
                    "conversation_not_found",
                    $"Conversation with ID '{request.ConversationId}' does not exist."
                );
            }

            // Step 2: Verify requesting user is a participant
            var isParticipant = await _context.ConversationParticipants
                .AsNoTracking()
                .AnyAsync(
                    cp => cp.ConversationId == request.ConversationId && 
                          cp.UserId == request.RequestingUserId,
                    cancellationToken);

            if (!isParticipant)
            {
                return BaseResponse<GetConversationMessagesResponse>.Failure(
                    "Failed to retrieve messages.",
                    "not_a_participant",
                    "You are not a participant in this conversation."
                );
            }

            // Step 3: Ensure page size is within bounds
            var pageSize = Math.Min(request.PageSize, 100);
            pageSize = Math.Max(pageSize, 1);

            // Step 4: Fetch messages using cursor-based pagination with IContext
            // BeforeMessageId acts as a cursor (load messages before this ID)
            var messagesQuery = _context.Messages
                .AsNoTracking()
                .Where(m => m.ConversationId == request.ConversationId && !m.IsDeleted);

            if (request.BeforeMessageId.HasValue)
            {
                messagesQuery = messagesQuery.Where(m => m.Id < request.BeforeMessageId.Value);
            }

            var messageList = await messagesQuery
                .OrderByDescending(m => m.Id)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            // Step 5: Get total count for statistics
            var totalCount = await _context.Messages
                .AsNoTracking()
                .CountAsync(
                    m => m.ConversationId == request.ConversationId && !m.IsDeleted,
                    cancellationToken);

            // Step 6: Build response DTOs
            var messageDtos = messageList
                .Select(m => new ConversationMessageDto
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    Text = m.Content.Text,
                    AttachmentUrls = m.Content.AttachmentUrls,
                    EmbedJsonData = m.Content.EmbedJsonData,
                    Type = (int)m.Type,
                    RepliedToMessageId = m.RepliedToMessageId,
                    MentionedUserIds = m.MentionedUserIds,
                    CreatedAt = m.CreatedAt,
                    UpdatedAt = m.UpdatedAt,
                    IsDeleted = m.IsDeleted
                })
                .ToList();

            // Step 7: Determine if there are more messages to load
            var oldestMessageId = messageList.LastOrDefault()?.Id;
            var hasMoreMessages = false;

            if (oldestMessageId.HasValue)
            {
                // Check if there are messages before the oldest in this batch
                hasMoreMessages = await _context.Messages
                    .AsNoTracking()
                    .AnyAsync(
                        m => m.ConversationId == request.ConversationId && 
                             m.Id < oldestMessageId.Value && 
                             !m.IsDeleted,
                        cancellationToken);
            }

            // Step 8: Build and return response
            var response = new GetConversationMessagesResponse
            {
                Messages = messageDtos.AsReadOnly(),
                TotalCount = totalCount,
                OldestMessageId = oldestMessageId,
                HasMoreMessages = hasMoreMessages
            };

            return BaseResponse<GetConversationMessagesResponse>.Success(
                response,
                $"Retrieved {messageDtos.Count} messages."
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




