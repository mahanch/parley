using MediatR;
using Microsoft.EntityFrameworkCore;
using Parley.Application._Shared.DTOs;
using Parley.Application.Contracts.Interfaces.Data;
using Parley.Application.Contracts.Interfaces.Infrastructure;
using Parley.Application.Features.Messages.Commands;
using Parley.Domain._Shared;
using Parley.Domain.Aggregates.ConversationAgg;
using Parley.Domain.Aggregates.MessageAgg;
using Parley.Domain.Aggregates.MessageAgg.Entities;
using Parley.Domain.Aggregates.MessageAgg.Enums;
using Parley.Domain.Aggregates.MessageAgg.ValueObjects;

namespace Parley.Application.Features.Messages.Handlers;

/// <summary>
/// Handler for SendMessageCommand.
/// Demonstrates:
/// - Snowflake ID generation
/// - Domain entity creation
/// - Repository usage for writes (commands)
/// - IContext usage for reads (permission checks)
/// - Unit of Work pattern
/// - Error handling
/// </summary>
public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, BaseResponse<SendMessageResponse>>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IConversationRepository _conversationRepository;
    private readonly IContext _context;
    private readonly ISnowflakeIdGenerator _snowflakeIdGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public SendMessageCommandHandler(
        IMessageRepository messageRepository,
        IConversationRepository conversationRepository,
        IContext context,
        ISnowflakeIdGenerator snowflakeIdGenerator,
        IUnitOfWork unitOfWork)
    {
        _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
        _conversationRepository = conversationRepository ?? throw new ArgumentNullException(nameof(conversationRepository));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _snowflakeIdGenerator = snowflakeIdGenerator ?? throw new ArgumentNullException(nameof(snowflakeIdGenerator));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<BaseResponse<SendMessageResponse>> Handle(
        SendMessageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Step 1: Verify that the conversation exists using IContext (read-optimized)
            var conversationExists = await _context.Conversations
                .AsNoTracking()
                .AnyAsync(c => c.Id == request.ConversationId, cancellationToken);

            if (!conversationExists)
            {
                return BaseResponse<SendMessageResponse>.Failure(
                    "Failed to send message.",
                    "conversation_not_found",
                    $"Conversation with ID '{request.ConversationId}' does not exist."
                );
            }

            // Step 2: Verify sender is a participant using IContext (read-optimized)
            var isParticipant = await _context.ConversationParticipants
                .AsNoTracking()
                .AnyAsync(
                    cp => cp.ConversationId == request.ConversationId && 
                          cp.UserId == request.SenderId,
                    cancellationToken);

            if (!isParticipant)
            {
                return BaseResponse<SendMessageResponse>.Failure(
                    "Failed to send message.",
                    "not_a_participant",
                    "You are not a participant in this conversation."
                );
            }

            // Step 3: Create MessageContent Value Object
            var messageContent = MessageContent.Create(request.Text,request.AttachmentUrls, request.EmbedJsonData);

            // Step 4: Generate Snowflake ID for the message
            // This is crucial for chronological ordering at scale
            var messageId = _snowflakeIdGenerator.GenerateId();

            // Step 5: Create the Message aggregate
            var message = new Message(
                id: messageId,
                conversationId: request.ConversationId,
                senderId: request.SenderId,
                content: messageContent,
                type: MessageType.Default,
                repliedToMessageId: request.RepliedToMessageId,
                mentionedUserIds: request.MentionedUserIds ?? new List<Guid>()
            );

            // Step 6: Add message to repository (write operation)
            await _messageRepository.AddAsync(message, cancellationToken);

            // Step 7: Update participant's watermark (last read message)
            // Fetch participant using repository to get tracked entity for update
            var participant = await _context.ConversationParticipants
                .FirstOrDefaultAsync(
                    cp => cp.ConversationId == request.ConversationId && 
                          cp.UserId == request.SenderId,
                    cancellationToken);

            if (participant != null)
            {
                participant.UpdateWatermark(messageId);
            }

            // Step 8: Save all changes atomically
            var savedRows = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (savedRows <= 0)
            {
                return BaseResponse<SendMessageResponse>.Failure(
                    "Failed to save message to database."
                );
            }

            // Step 9: Build and return response
            var response = new SendMessageResponse
            {
                MessageId = message.Id,
                CreatedAt = message.CreatedAt,
                ConversationId = message.ConversationId,
                SenderId = message.SenderId,
                Text = message.Content.Text
            };

            return BaseResponse<SendMessageResponse>.Success(
                response,
                "Message sent successfully."
            );
        }
        catch (Exception ex)
        {
            // In production, log this exception
            return BaseResponse<SendMessageResponse>.Failure(
                "An unexpected error occurred while sending the message.",
                "internal_error",
                ex.Message
            );
        }
    }
}


