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
/// Handler for CreateGroupConversationCommand.
/// Creates a group conversation with initial participants.
/// </summary>
public class CreateGroupConversationCommandHandler : IRequestHandler<CreateGroupConversationCommand, BaseResponse<CreateConversationResponse>>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGroupConversationCommandHandler(
        IConversationRepository conversationRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _conversationRepository = conversationRepository ?? throw new ArgumentNullException(nameof(conversationRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<BaseResponse<CreateConversationResponse>> Handle(
        CreateGroupConversationCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Validate creator exists
            var creatorExists = await _userRepository.ExistsAsync(request.CreatorId, cancellationToken);
            if (!creatorExists)
            {
                return BaseResponse<CreateConversationResponse>.Failure(
                    "Failed to create group conversation.",
                    ErrorType.BadRequest,
                    "creator_not_found",
                    "Creator user does not exist."
                );
            }

            // Validate all participants exist
            var participantIds = new List<Guid>(request.ParticipantIds) { request.CreatorId };
            var uniqueParticipantIds = participantIds.Distinct().ToList();

            foreach (var userId in uniqueParticipantIds)
            {
                var exists = await _userRepository.ExistsAsync(userId, cancellationToken);
                if (!exists)
                {
                    return BaseResponse<CreateConversationResponse>.Failure(
                        "Failed to create group conversation.",
                        ErrorType.BadRequest,
                        "participant_not_found",
                        $"Participant with ID '{userId}' does not exist."
                    );
                }
            }

            // Create group conversation
            var conversation = new Conversation(ConversationType.Group, request.Name);

            // Add participants (creator as admin, others as members)
            foreach (var userId in uniqueParticipantIds)
            {
                var role = userId == request.CreatorId ? GroupRole.Admin : GroupRole.Member;
                conversation.AddParticipant(userId, role);
            }

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
            var response = new CreateConversationResponse
            {
                ConversationId = conversation.Id,
                Type = conversation.Type.ToString(),
                Name = conversation.Name,
                CreatedAt = conversation.CreatedAt
            };

            return BaseResponse<CreateConversationResponse>.Success(
                response,
                "Group conversation created successfully."
            );
        }
        catch (Exception ex)
        {
            return BaseResponse<CreateConversationResponse>.Failure(
                "An unexpected error occurred while creating the group conversation.",
                ErrorType.BadRequest,
                "internal_error",
                ex.Message
            );
        }
    }
}
