namespace Parley.Application.Contracts.Query.Conversation;

public interface IConversationQueryService:IQueryService<Guid,Domain.Aggregates.ConversationAgg.Entities.Conversation>
{
    Task<GetConversationMessagesResult?> GetMessagesAsync(GetConversationMessagesFilter  filter, CancellationToken ct);
    Task<IReadOnlyList<UserConversationDto>> GetUserConversationsAsync(Guid userId, CancellationToken ct);
}

public class GetConversationMessagesFilter
{
    public Guid ConversationId { get; init; }
    public Guid RequestingUserId { get; init; }
    public int PageSize { get; init; }
    public long? BeforeMessageId { get; init; }
}

public record GetConversationMessagesResult
{
    public IReadOnlyCollection<ConversationMessageDto>? Messages { get; set; }
    public int TotalCount { get; set; }
    public long? OldestMessageId { get; set; }
    public bool HasMoreMessages { get; set; }
    public string? ErrorCode { get; set; } // اگر خواستی به Handler منتقلش کنی
}
public class ConversationMessageDto
{
    public long Id { get; set; }
    public Guid SenderId { get; set; }
    public string? Text { get; set; }
    public IReadOnlyList<string>? AttachmentUrls { get; set; }
    public string? EmbedJsonData { get; set; }
    public int Type { get; set; }
    public long? RepliedToMessageId { get; set; }
    public IReadOnlyList<Guid>? MentionedUserIds { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}

public class UserConversationDto
{
    public Guid ConversationId { get; set; }
    public string Title { get; set; } = null!;
    public string? LastMessagePreview { get; set; }
    public DateTime? LastMessageTime { get; set; }
    public bool IsPinned { get; set; }
    public bool IsMuted { get; set; }
    public int UnreadMessagesCount { get; set; }
}
