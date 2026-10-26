using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Ticketing.Queries.Implementations
{
    internal sealed class TicketStatusQueryService : ITicketStatusQueryService
    {
        private readonly DbSet<TicketStatus> _statuses;

        public TicketStatusQueryService(IUnitOfWork uow)
        {
            uow.NotNull(nameof(uow));
            _statuses = uow.Set<TicketStatus>();
        }

        public async Task<ICollection<TicketStatus>> Get(CancellationToken cancellationToken = default)
            => await _statuses.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

        public Task<TicketStatus?> Get(short id, CancellationToken cancellationToken = default)
            => _statuses.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
