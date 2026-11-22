using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Repositories
{
    internal sealed class HandoverRepository : IHandoverCommandService, IHandoverQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Handover> _handover;
        public HandoverRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _handover = _uow.Set<Handover>();
            _handover.NotNull(nameof(_handover));
        }

        public async Task Add(Handover handover)
        {
            await _handover.AddAsync(handover);
        }

        public async Task Remove(Handover handover)
        {
            _handover.Remove(handover);
        }

        public async Task<Handover> Get(short id)
        {
            return await _uow.FindOrThrowAsync<Handover>(id);
        }

        public async Task<ICollection<Handover>> Get()
        {
            return await _handover.ToListAsync();
        }

    }
}
