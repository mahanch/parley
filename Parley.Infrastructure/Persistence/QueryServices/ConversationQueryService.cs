using Microsoft.EntityFrameworkCore;
using Parley.Application.Contracts.Query.Conversation;
using Parley.Application.Features.Messages.Queries;
using Parley.Domain.Aggregates.ConversationAgg.Entities;

namespace Parley.Infrastructure.Persistence.QueryServices;

public class ConversationQueryService:BaseQueryService<Guid,Conversation>,IConversationQueryService
{
    private readonly ParleyDbContext _context;
    public ConversationQueryService(ParleyDbContext dbContext) : base(dbContext)
    {
        _context = dbContext;
    }

    public async Task<GetConversationMessagesResult?> GetMessagesAsync(GetConversationMessagesFilter filter, CancellationToken cancellationToken)
    {
        //    // STEP 1: conversation موجود؟
        // var conversationExists = await _context.Conversations
        //     .AsNoTracking()
        //     .AnyAsync(c => c.Id == filter.ConversationId, cancellationToken);
        //
        // if (!conversationExists)
        //     return null;
        //
        // // STEP 2: کاربر جزو شرکت‌کنندگان است؟
        // var isParticipant = await _context.ConversationParticipants
        //     .AsNoTracking()
        //     .AnyAsync(
        //         cp => cp.ConversationId == filter.ConversationId &&
        //               cp.UserId == filter.RequestingUserId,
        //         cancellationToken);
        //
        // if (!isParticipant)
        //     return new GetConversationMessagesResult
        //     {
        //         ErrorCode = "not_a_participant"
        //     };
        //
        // // STEP 3: محدودسازی page size
        // var pageSize = Math.Clamp(filter.PageSize, 1, 100);
        //
        // // STEP 4: کوئری پیام‌ها
        // var messagesQuery = _context.Messages
        //     .AsNoTracking()
        //     .Where(m => m.ConversationId == request.ConversationId && !m.IsDeleted);
        //
        // if (request.BeforeMessageId.HasValue)
        //     messagesQuery = messagesQuery.Where(m => m.Id < request.BeforeMessageId.Value);
        //
        // var messageList = await messagesQuery
        //     .OrderByDescending(m => m.Id)
        //     .Take(pageSize)
        //     .ToListAsync(cancellationToken);
        //
        // // STEP 5: شمارش کل پیام‌ها
        // var totalCount = await _context.Messages
        //     .AsNoTracking()
        //     .CountAsync(
        //         m => m.ConversationId == filter.ConversationId && !m.IsDeleted,
        //         cancellationToken);
        //
        // // STEP 6: ساخت DTO‌ها
        // var messageDtos = messageList.Select(m => new ConversationMessageDto
        // {
        //     Id = m.Id,
        //     SenderId = m.SenderId,
        //     Text = m.Content.Text,
        //     AttachmentUrls = m.Content.AttachmentUrls,
        //     EmbedJsonData = m.Content.EmbedJsonData,
        //     Type = (int)m.Type,
        //     RepliedToMessageId = m.RepliedToMessageId,
        //     MentionedUserIds = m.MentionedUserIds,
        //     CreatedAt = m.CreatedAt,
        //     UpdatedAt = m.UpdatedAt,
        //     IsDeleted = m.IsDeleted
        // }).ToList();
        //
        // // STEP 7: آیا پیام بیشتری وجود دارد؟
        // var oldestMessageId = messageList.LastOrDefault()?.Id;
        // var hasMoreMessages = oldestMessageId.HasValue &&
        //     await _context.Messages
        //         .AsNoTracking()
        //         .AnyAsync(
        //             m => m.ConversationId == filter.ConversationId &&
        //                  m.Id < oldestMessageId.Value &&
        //                  !m.IsDeleted,
        //             cancellationToken);
        //
        // // STEP 8: خروجی compose شده
        // return new GetConversationMessagesResult
        // {
        //     Messages = messageDtos.AsReadOnly(),
        //     TotalCount = totalCount,
        //     OldestMessageId = oldestMessageId,
        //     HasMoreMessages = hasMoreMessages
        // };
        throw new NotImplementedException();
    }
}