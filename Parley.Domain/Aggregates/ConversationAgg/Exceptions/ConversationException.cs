using Parley.Domain._Shared.Exceptions;

namespace Parley.Domain.Aggregates.ConversationAgg.Exceptions;

/// <summary>
/// Exception specific to Conversation aggregate business rule violations.
/// </summary>
public class ConversationException : DomainException
{
    public ConversationException(string message) : base(message)
    {
    }

    public ConversationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

