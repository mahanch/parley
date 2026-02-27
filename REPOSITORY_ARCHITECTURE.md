# Repository Architecture - Summary

## Architecture Overview

This project follows **Domain-Driven Design (DDD)** principles where:

✅ **Repository INTERFACES are in the DOMAIN layer** (Correct DDD approach)
✅ **Repository IMPLEMENTATIONS are in the INFRASTRUCTURE layer**

## Why Repository Interfaces in Domain?

1. **Dependency Inversion Principle**: Domain defines contracts, Infrastructure implements them
2. **Domain Independence**: Domain doesn't depend on Infrastructure
3. **Ubiquitous Language**: Repositories are part of the domain model
4. **Testability**: Easy to mock repositories in tests

## Structure

### Domain Layer (`Parley.Domain`)

```
Domain/
├── _Shared/
│   ├── IRepository.cs              ← Generic base repository interface
│   ├── IUnitOfWork.cs             ← Unit of Work interface
│   ├── EntityBase.cs
│   └── AggregateRoot.cs
└── Aggregates/
    ├── ConversationAgg/
    │   ├── IConversationRepository.cs  ← Specific interface
    │   └── Entities/
    │       └── Conversation.cs
    ├── MessageAgg/
    │   ├── IMessageRepository.cs       ← Specific interface
    │   └── Entities/
    │       └── Message.cs
    └── ServerAgg/
        ├── IServerRepository.cs        ← Specific interface
        └── Entities/
            └── Server.cs
```

### Infrastructure Layer (`Parley.Infrastructure`)

```
Infrastructure/
└── Persistence/
    ├── ParleyDbContext.cs
    └── Repositories/
        ├── RepositoryBase.cs              ← Generic base implementation
        ├── ConversationRepository.cs      ← Implements IConversationRepository
        ├── MessageRepository.cs           ← Implements IMessageRepository
        ├── ServerRepository.cs            ← Implements IServerRepository
        └── UnitOfWork.cs                  ← Implements IUnitOfWork
```

## Key Design Decisions

### 1. Generic Repository Pattern

**Interface (Domain):**
```csharp
public interface IRepository<TEntity, TKey> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, ...);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    // ... more methods
}
```

**Implementation (Infrastructure):**
```csharp
public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
{
    protected readonly ParleyDbContext Context;
    protected readonly DbSet<TEntity> DbSet;
    // ... implementations
}
```

### 2. Specific Repository Interfaces

Each aggregate has its own specific repository interface:

```csharp
// In Domain/Aggregates/ConversationAgg/
public interface IConversationRepository : IRepository<Conversation, Guid>
{
    Task<IEnumerable<Conversation>> GetUserConversationsAsync(...);
    // ... domain-specific queries
}
```

### 3. Concrete Repository Implementations

```csharp
// In Infrastructure/Persistence/Repositories/
public sealed class ConversationRepository : RepositoryBase<Conversation, Guid>, IConversationRepository
{
    public ConversationRepository(ParleyDbContext context) : base(context) { }
    // ... implement specific methods
}
```

### 4. Unit of Work Pattern

**Interface (Domain):**
- Manages transactions
- Coordinates saves across repositories

**Implementation (Infrastructure):**
- Wraps DbContext transaction management
- Ensures atomic operations

## Usage in Application Layer

```csharp
public class SendMessageCommandHandler
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new Message(...);
        await _messageRepository.AddAsync(message, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
```

## Dependency Flow

```
Domain (Interfaces)
   ↑
   | depends on
   |
Application (Uses Interfaces)
   ↑
   | depends on
   |
Infrastructure (Implements Interfaces)
```

## Key Points

- ✅ Domain defines **WHAT** it needs (interfaces)
- ✅ Infrastructure provides **HOW** it works (implementations)
- ✅ Application uses interfaces, doesn't know about implementations
- ✅ SaveChanges is done via UnitOfWork (not in repositories)
- ✅ Update/Delete are synchronous (only mark for changes)
- ✅ Only Add operations are async (for key generation scenarios)

## Registration (Infrastructure DI)

```csharp
services.AddScoped<IConversationRepository, ConversationRepository>();
services.AddScoped<IMessageRepository, MessageRepository>();
services.AddScoped<IServerRepository, ServerRepository>();
services.AddScoped<IUnitOfWork, UnitOfWork>();
```

This architecture is **clean, maintainable, and follows DDD best practices**.

