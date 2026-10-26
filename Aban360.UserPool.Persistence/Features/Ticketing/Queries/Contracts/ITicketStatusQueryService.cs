using Aban360.UserPool.Domain.Features.Ticketing.Entities;

namespace Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts
{
    public interface ITicketStatusQueryService
    {
        Task<ICollection<TicketStatus>> Get(CancellationToken cancellationToken = default);
        Task<TicketStatus?> Get(short id, CancellationToken cancellationToken = default);
    }
}
