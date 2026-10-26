using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;

namespace Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts
{
    public interface ITicketMessageQueryService
    {
        Task<TicketMessage?> Get(long id, CancellationToken cancellationToken = default);
        Task<TicketPage<TicketMessage>> GetByTicket(long ticketId, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);
    }
}
