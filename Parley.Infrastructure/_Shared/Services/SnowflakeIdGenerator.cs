using IdGen;

namespace Parley.Infrastructure._Shared.Services;

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

/// <summary>
/// Default implementation of Snowflake ID generation.
/// </summary>
public class SnowflakeIdGenerator : ISnowflakeIdGenerator
{
    private readonly IIdGenerator<long> _generator;

    /// <summary>
    /// Creates a new instance of SnowflakeIdGenerator.
    /// </summary>
    /// <param name="generatorId">The ID of this generator (0-1023). Used in distributed systems to ensure uniqueness.</param>
    public SnowflakeIdGenerator(int generatorId = 0)
    {
        if (generatorId < 0 || generatorId > 1023)
            throw new ArgumentException("Generator ID must be between 0 and 1023.", nameof(generatorId));

        _generator = new IdGenerator(generatorId);
    }

    /// <summary>
    /// Generates a new Snowflake ID.
    /// </summary>
    public long GenerateId()
    {
        return _generator.CreateId();
    }
}

