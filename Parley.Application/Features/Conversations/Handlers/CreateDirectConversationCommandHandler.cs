using MediatR;
using Parley.Application._Shared.DTOs;
using Parley.Application.Contracts.Interfaces.Infrastructure;
using Parley.Application.Features.Conversations.Commands;
using Parley.Domain._Shared;
using Parley.Domain.Aggregates.ConversationAgg;
using Parley.Domain.Aggregates.ConversationAgg.Entities;
using Parley.Domain.Aggregates.ConversationAgg.Enums;
using Parley.Domain.Aggregates.UserAgg;

namespace Parley.Application.Features.Conversations.Handlers;

/// <summary>
/// Handler for CreateDirectConversationCommand.
/// Creates a direct conversation between two users.
/// If a direct conversation already exists between them, returns the existing one.
/// </summary>
public class CreateDirectConversationCommandHandler : IRequestHandler<CreateDirectConversationCommand, BaseResponse<CreateConversationResponse>>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDirectConversationCommandHandler(
        IConversationRepository conversationRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _conversationRepository = conversationRepository ?? throw new ArgumentNullException(nameof(conversationRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<BaseResponse<CreateConversationResponse>> Handle(
        CreateDirectConversationCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Validate users exist
            var user1Exists = await _userRepository.ExistsAsync(request.UserId1, cancellationToken);
            var user2Exists = await _userRepository.ExistsAsync(request.UserId2, cancellationToken);

            if (!user1Exists || !user2Exists)
            {
                return BaseResponse<CreateConversationResponse>.Failure(
                    "Failed to create direct conversation.",
                    ErrorType.BadRequest,
                    "user_not_found",
                    "One or both users do not exist."
                );
            }

            // Check if direct conversation already exists between these users
            var existingConversationId = await _conversationRepository.FindDirectConversationAsync(
                request.UserId1, request.UserId2, cancellationToken);

            if (existingConversationId.HasValue)
            {
                var existingConversation = await _conversationRepository.GetByIdAsync(existingConversationId.Value, cancellationToken);
                if (existingConversation == null)
                {
                    return BaseResponse<CreateConversationResponse>.Failure(
                        "Failed to retrieve existing conversation."
                    );
                }

                var response = new CreateConversationResponse
                {
                    ConversationId = existingConversation.Id,
                    Type = existingConversation.Type.ToString(),
                    Name = existingConversation.Name,
                    CreatedAt = existingConversation.CreatedAt
                };

                return BaseResponse<CreateConversationResponse>.Success(
                    response,
                    "Direct conversation already exists."
                );
            }

            // Create new direct conversation
            var conversation = new Conversation(ConversationType.Direct);

            // Add both users as participants
            conversation.AddParticipant(request.UserId1);
            conversation.AddParticipant(request.UserId2);

            // Save to repository
            await _conversationRepository.AddAsync(conversation, cancellationToken);
            var savedRows = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (savedRows <= 0)
            {
                return BaseResponse<CreateConversationResponse>.Failure(
                    "Failed to save conversation to database."
                );
            }

            // Build response
            var createResponse = new CreateConversationResponse
            {
                ConversationId = conversation.Id,
                Type = conversation.Type.ToString(),
                Name = conversation.Name,
                CreatedAt = conversation.CreatedAt
            };

            return BaseResponse<CreateConversationResponse>.Success(
                createResponse,
                "Direct conversation created successfully."
            );
        }
        catch (Exception ex)
        {
            return BaseResponse<CreateConversationResponse>.Failure(
                "An unexpected error occurred while creating the direct conversation.",
                ErrorType.BadRequest,
                "internal_error",
                ex.Message
            );
        }
    }
}
