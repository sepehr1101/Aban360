using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;

namespace Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts
{
    public interface ITicketAttachmentQueryService
    {
        Task<int> CountByMessage(long messageId, CancellationToken cancellationToken = default);
        Task<TicketAttachment?> GetForUpdate(long id, CancellationToken cancellationToken = default);
        Task<TicketAttachmentInfo?> GetInfo(long id, CancellationToken cancellationToken = default);
        Task<ICollection<TicketAttachmentInfo>> GetByMessage(long messageId, CancellationToken cancellationToken = default);
        // Loads file bytes only when explicitly requested, after application authorization.
        Task<TicketAttachment?> GetContent(long id, CancellationToken cancellationToken = default);
    }
}
