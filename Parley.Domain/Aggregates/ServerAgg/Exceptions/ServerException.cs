using Parley.Domain._Shared.Exceptions;

namespace Parley.Domain.Aggregates.ServerAgg.Exceptions;

/// <summary>
/// Exception specific to Server aggregate business rule violations.
/// </summary>
public class ServerException : DomainException
{
    public ServerException(string message) : base(message)
    {
    }

    public ServerException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

