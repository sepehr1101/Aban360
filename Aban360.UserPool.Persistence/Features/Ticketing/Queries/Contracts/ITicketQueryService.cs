using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;

namespace Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts
{
    public interface ITicketQueryService
    {
        // Tracked entity for command handlers; null when not found.
        Task<Ticket?> Get(long id, CancellationToken cancellationToken = default);
        Task<TicketPage<Ticket>> Get(TicketSearchInput input, CancellationToken cancellationToken = default);
    }
}
