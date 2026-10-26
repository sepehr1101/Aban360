using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Ticketing.Queries.Implementations
{
    internal sealed class TicketQueryService : ITicketQueryService
    {
        private readonly DbSet<Ticket> _tickets;

        public TicketQueryService(IUnitOfWork uow)
        {
            uow.NotNull(nameof(uow));
            _tickets = uow.Set<Ticket>();
        }

        public Task<Ticket?> Get(long id, CancellationToken cancellationToken = default)
            => _tickets.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task<TicketPage<Ticket>> Get(TicketSearchInput input, CancellationToken cancellationToken = default)
        {
            input.NotNull(nameof(input));
            var skip = TicketQueryFilter.GetSkip(input.PageNumber, input.PageSize);
            var query = TicketQueryFilter.Apply(_tickets.AsNoTracking(), input);
            return new TicketPage<Ticket>
            {
                TotalCount = await query.CountAsync(cancellationToken),
                Items = await query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
                    .Skip(skip).Take(input.PageSize).ToListAsync(cancellationToken),
                PageNumber = input.PageNumber,
                PageSize = input.PageSize
            };
        }
    }
}
