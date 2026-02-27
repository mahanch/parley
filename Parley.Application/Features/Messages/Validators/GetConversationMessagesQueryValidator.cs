using FluentValidation;
using Parley.Application.Features.Messages.Queries;

namespace Parley.Application.Features.Messages.Validators;

/// <summary>
/// Validator for GetConversationMessagesQuery.
/// </summary>
public class GetConversationMessagesQueryValidator : AbstractValidator<GetConversationMessagesQuery>
{
    public GetConversationMessagesQueryValidator()
    {
        // ConversationId validation
        RuleFor(x => x.ConversationId)
            .NotEmpty()
            .WithMessage("Conversation ID cannot be empty.")
            .WithErrorCode("CONVERSATION_ID_EMPTY");

        // RequestingUserId validation
        RuleFor(x => x.RequestingUserId)
            .NotEmpty()
            .WithMessage("Requesting user ID cannot be empty.")
            .WithErrorCode("USER_ID_EMPTY");

        // PageSize validation
        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.")
            .WithErrorCode("PAGE_SIZE_INVALID")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size cannot exceed 100.")
            .WithErrorCode("PAGE_SIZE_TOO_LARGE");

        // BeforeMessageId validation (if provided)
        RuleFor(x => x.BeforeMessageId)
            .GreaterThan(0)
            .WithMessage("Before message ID must be a valid Snowflake ID.")
            .WithErrorCode("INVALID_CURSOR")
            .When(x => x.BeforeMessageId.HasValue);
    }
}

