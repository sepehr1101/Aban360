using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Ticketing.Queries.Implementations
{
    internal sealed class TicketRatingQueryService : ITicketRatingQueryService
    {
        private readonly DbSet<TicketRating> _ratings;

        public TicketRatingQueryService(IUnitOfWork uow)
        {
            uow.NotNull(nameof(uow));
            _ratings = uow.Set<TicketRating>();
        }

        public Task<TicketRating?> Get(long id, CancellationToken cancellationToken = default)
            => _ratings.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        public Task<TicketRating?> GetByMessage(long messageId, CancellationToken cancellationToken = default)
            => _ratings.SingleOrDefaultAsync(x => x.TicketMessageId == messageId, cancellationToken);
    }
}
