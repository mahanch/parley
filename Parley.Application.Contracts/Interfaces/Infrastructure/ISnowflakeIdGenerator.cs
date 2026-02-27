namespace Parley.Application.Contracts.Interfaces.Infrastructure;

/// <summary>
/// Generates Snowflake IDs (long) for high-scale chronological sorting.
/// Uses the IdGen library to generate unique, time-ordered IDs.
/// </summary>
public interface ISnowflakeIdGenerator
{
    /// <summary>
    /// Generates a new Snowflake ID.
    /// </summary>
    long GenerateId();
}

