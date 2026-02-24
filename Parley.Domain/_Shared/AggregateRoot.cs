namespace Parley.Domain._Shared;

/// <summary>
/// Base class for all aggregate roots in the domain.
/// Aggregate roots are the entry points to aggregates and control access to entities within.
/// </summary>
public abstract class AggregateRoot<T> : EntityBase<T>
{
}


