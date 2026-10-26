using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Ticketing.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Ticketing.Queries.Implementations
{
    internal sealed class TicketMessageQueryService : ITicketMessageQueryService
    {
        private readonly DbSet<TicketMessage> _messages;

        public TicketMessageQueryService(IUnitOfWork uow)
        {
            uow.NotNull(nameof(uow));
            _messages = uow.Set<TicketMessage>();
        }

        public Task<TicketMessage?> Get(long id, CancellationToken cancellationToken = default)
            => _messages.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task<TicketPage<TicketMessage>> GetByTicket(long ticketId, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var skip = TicketQueryFilter.GetSkip(pageNumber, pageSize);
            var query = _messages.AsNoTracking().Where(x => x.TicketId == ticketId);
            return new TicketPage<TicketMessage>
            {
                TotalCount = await query.CountAsync(cancellationToken),
                Items = await query.OrderBy(x => x.CreatedAt).ThenBy(x => x.Id)
                    .Skip(skip).Take(pageSize).ToListAsync(cancellationToken),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
