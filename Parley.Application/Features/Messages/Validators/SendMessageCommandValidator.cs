using FluentValidation;
using Parley.Application.Features.Messages.Commands;

namespace Parley.Application.Features.Messages.Validators;

/// <summary>
/// Validator for SendMessageCommand using FluentValidation.
/// Ensures data integrity before the command is processed.
/// </summary>
public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        // ConversationId validation
        RuleFor(x => x.ConversationId)
            .NotEmpty()
            .WithMessage("Conversation ID cannot be empty.")
            .WithErrorCode("CONVERSATION_ID_EMPTY");

        // SenderId validation
        RuleFor(x => x.SenderId)
            .NotEmpty()
            .WithMessage("Sender ID cannot be empty.")
            .WithErrorCode("SENDER_ID_EMPTY");

        // Text validation
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage("Message text cannot be empty.")
            .WithErrorCode("TEXT_EMPTY")
            .MaximumLength(2000)
            .WithMessage("Message text cannot exceed 2000 characters.")
            .WithErrorCode("TEXT_TOO_LONG")
            .Must(text => !string.IsNullOrWhiteSpace(text))
            .WithMessage("Message text cannot contain only whitespace.")
            .WithErrorCode("TEXT_WHITESPACE_ONLY");

        // AttachmentUrls validation (if provided)
        RuleFor(x => x.AttachmentUrls)
            .Must(attachments => attachments == null || attachments.Count <= 10)
            .WithMessage("Cannot attach more than 10 files.")
            .WithErrorCode("TOO_MANY_ATTACHMENTS")
            .When(x => x.AttachmentUrls != null);

        RuleForEach(x => x.AttachmentUrls)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("'{PropertyValue}' is not a valid URL.")
            .WithErrorCode("INVALID_URL")
            .When(x => x.AttachmentUrls != null);

        // EmbedJsonData validation (if provided)
        RuleFor(x => x.EmbedJsonData)
            .MaximumLength(5000)
            .WithMessage("Embed data cannot exceed 5000 characters.")
            .WithErrorCode("EMBED_DATA_TOO_LONG")
            .When(x => !string.IsNullOrEmpty(x.EmbedJsonData));

        // RepliedToMessageId validation (if provided)
        RuleFor(x => x.RepliedToMessageId)
            .GreaterThan(0)
            .WithMessage("Replied message ID must be a valid Snowflake ID.")
            .WithErrorCode("INVALID_REPLY_MESSAGE_ID")
            .When(x => x.RepliedToMessageId.HasValue);

        // MentionedUserIds validation (if provided)
        RuleFor(x => x.MentionedUserIds)
            .Must(mentions => mentions == null || mentions.Count <= 50)
            .WithMessage("Cannot mention more than 50 users.")
            .WithErrorCode("TOO_MANY_MENTIONS")
            .When(x => x.MentionedUserIds != null);

        RuleForEach(x => x.MentionedUserIds)
            .NotEmpty()
            .WithMessage("Mentioned user ID cannot be empty.")
            .WithErrorCode("EMPTY_MENTION_ID")
            .When(x => x.MentionedUserIds != null);
    }
}

