using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Ticketing.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Ticketing.Commands.Implementations
{
    internal sealed class TicketMessageCommandService : ITicketMessageCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<TicketMessage> _entities;

        public TicketMessageCommandService(IUnitOfWork uow)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            _entities = uow.Set<TicketMessage>();
        }

        public async Task Add(TicketMessage entity, CancellationToken cancellationToken = default)
        {
            entity.NotNull(nameof(entity));
            await _entities.AddAsync(entity, cancellationToken);
        }

        public void Update(TicketMessage entity)
        {
            entity.NotNull(nameof(entity));
            // Update scalar values only; do not mark the entire navigation graph modified.
            _uow.GetEntry(entity).State = EntityState.Modified;
        }
    }
}
