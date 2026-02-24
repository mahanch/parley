using Parley.Domain._Shared.Exceptions;

namespace Parley.Domain.Aggregates.MessageAgg.Exceptions;

/// <summary>
/// Exception specific to Message aggregate business rule violations.
/// </summary>
public class MessageException : DomainException
{
    public MessageException(string message) : base(message)
    {
    }

    public MessageException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

