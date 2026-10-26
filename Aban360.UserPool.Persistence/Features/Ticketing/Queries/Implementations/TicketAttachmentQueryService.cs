using Aban360.Common.Extensions;
using System.Linq.Expressions;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Ticketing.Queries.Implementations
{
    internal sealed class TicketAttachmentQueryService : ITicketAttachmentQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<TicketAttachment> _attachments;
        private static readonly Expression<Func<TicketAttachment, TicketAttachmentInfo>> Info = x => new TicketAttachmentInfo
        {
            Id = x.Id,
            TicketMessageId = x.TicketMessageId,
            OriginalFileName = x.OriginalFileName,
            ContentType = x.ContentType,
            SizeBytes = x.SizeBytes,
            CreatedAt = x.CreatedAt
        };

        public TicketAttachmentQueryService(IUnitOfWork uow)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            _attachments = uow.Set<TicketAttachment>();
        }

        public Task<TicketAttachmentInfo?> GetInfo(long id, CancellationToken cancellationToken = default)
            => _attachments.AsNoTracking().Where(x => x.Id == id).Select(Info).SingleOrDefaultAsync(cancellationToken);

        public async Task<int> CountByMessage(long messageId, CancellationToken cancellationToken = default)
        {
            var savedCount = await _attachments.CountAsync(x => x.TicketMessageId == messageId, cancellationToken);
            // A handler may stage several files before the caller commits the unit of work.
            return savedCount + _attachments.Local.Count(x => x.TicketMessageId == messageId
                && _uow.GetEntityState(x) == EntityState.Added);
        }

        public Task<TicketAttachment?> GetForUpdate(long id, CancellationToken cancellationToken = default)
            => _attachments.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task<ICollection<TicketAttachmentInfo>> GetByMessage(long messageId, CancellationToken cancellationToken = default)
            => await _attachments.AsNoTracking().Where(x => x.TicketMessageId == messageId)
                .OrderBy(x => x.Id).Select(Info).ToListAsync(cancellationToken);

        public Task<TicketAttachment?> GetContent(long id, CancellationToken cancellationToken = default)
            => _attachments.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
