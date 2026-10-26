using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Ticketing.Queries.Implementations
{
    internal sealed class TicketReportQueryService : ITicketReportQueryService
    {
        private readonly DbSet<Ticket> _tickets;
        private readonly DbSet<TicketRating> _ratings;

        public TicketReportQueryService(IUnitOfWork uow)
        {
            uow.NotNull(nameof(uow));
            _tickets = uow.Set<Ticket>();
            _ratings = uow.Set<TicketRating>();
        }

        public async Task<TicketReportOutput> Get(TicketSearchInput input, CancellationToken cancellationToken = default)
        {
            var tickets = TicketQueryFilter.Apply(_tickets.AsNoTracking(), input);
            var statusCounts = await tickets.GroupBy(x => x.StatusId)
                .Select(g => new TicketStatusCount { StatusId = g.Key, Count = g.Count() })
                .OrderBy(x => x.StatusId).ToListAsync(cancellationToken);
            // Count tickets separately from ratings: several rated replies must not
            // multiply the ticket count. An empty rating set has no average.
            var ratings = _ratings.AsNoTracking().Where(x => tickets.Select(t => t.Id).Contains(x.TicketMessage.TicketId));
            var ratingSummary = await ratings.GroupBy(x => 1)
                .Select(g => new { Count = g.Count(), Average = g.Average(x => (double)x.Score) })
                .SingleOrDefaultAsync(cancellationToken);

            return new TicketReportOutput
            {
                TicketCount = statusCounts.Sum(x => x.Count),
                StatusCounts = statusCounts,
                RatingCount = ratingSummary?.Count ?? 0,
                AverageScore = ratingSummary?.Average
            };
        }
    }
}
