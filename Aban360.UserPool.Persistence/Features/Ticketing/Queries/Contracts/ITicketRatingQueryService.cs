using Aban360.UserPool.Domain.Features.Ticketing.Entities;

namespace Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts
{
    public interface ITicketRatingQueryService
    {
        Task<TicketRating?> Get(long id, CancellationToken cancellationToken = default);
        Task<TicketRating?> GetByMessage(long messageId, CancellationToken cancellationToken = default);
    }
}
