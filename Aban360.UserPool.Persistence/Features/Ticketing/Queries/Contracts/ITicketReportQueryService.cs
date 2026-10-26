using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;

namespace Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts
{
    public interface ITicketReportQueryService
    {
        // Filters ticket creation time in UTC; includes ratings on those tickets.
        Task<TicketReportOutput> Get(TicketSearchInput input, CancellationToken cancellationToken = default);
    }
}
