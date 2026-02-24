namespace Parley.Domain._Shared;

/// <summary>
/// Base class for all entities in the domain.
/// </summary>
public abstract class EntityBase<T>
{
    public T Id { get; protected  init; }
    /// <summary>
    /// Timestamp when this entity was created.
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// Timestamp when this entity was last modified.
    /// </summary>
    public DateTime? UpdatedAt { get; protected set; }

    protected EntityBase()
    {
        if (typeof(T) == typeof(Guid))
        {
            Id = (T)(object)Guid.NewGuid();
        }
        CreatedAt = DateTime.UtcNow;
    }

    protected void MarkAsModified()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}

