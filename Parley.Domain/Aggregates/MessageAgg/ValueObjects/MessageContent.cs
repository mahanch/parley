namespace Parley.Domain.Aggregates.MessageAgg.ValueObjects;

/// <summary>
/// Represents the content of a message with text, attachments, and optional embed data.
/// This is a Value Object and will be stored as JSONB in PostgreSQL.
/// </summary>
public record MessageContent
{
    /// <summary>
    /// The main text content of the message.
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// URLs of attachments associated with this message.
    /// </summary>
    public required IReadOnlyList<string> AttachmentUrls { get; init; }

    /// <summary>
    /// Optional JSON data for rich embeds (links, images, etc.).
    /// This will be stored in PostgreSQL JSONB format.
    /// </summary>
    public string? EmbedJsonData { get; init; }

    /// <summary>
    /// Private constructor for EF Core.
    /// </summary>
    private MessageContent()
    {
        Text = string.Empty;
        AttachmentUrls = new List<string>();
    }

    /// <summary>
    /// Creates a new MessageContent with validation.
    /// </summary>
    public static MessageContent Create(string text, IReadOnlyList<string>? attachmentUrls = null, string? embedJsonData = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Message text cannot be empty.", nameof(text));

        if (text.Length > 4000)
            throw new ArgumentException("Message text cannot exceed 4000 characters.", nameof(text));

        return new MessageContent
        {
            Text = text.Trim(),
            AttachmentUrls = attachmentUrls ?? Array.Empty<string>(),
            EmbedJsonData = embedJsonData
        };
    }
}

