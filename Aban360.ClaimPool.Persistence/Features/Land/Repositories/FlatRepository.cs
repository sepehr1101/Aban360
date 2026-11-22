using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Repositories
{
    internal sealed class FlatRepository : IFlatCommandService, IFlatQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Flat> _flat;
        public FlatRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _flat = _uow.Set<Flat>();
            _flat.NotNull(nameof(_flat));
        }

        public async Task Add(Flat flat)
        {
            await _flat.AddAsync(flat);
        }

        public async Task Remove(Flat flat)
        {
            _flat.Remove(flat);
        }

        public async Task<Flat> Get(int id)
        {
            return await _flat
                .Include(f => f.Estate)
                .Where(f => f.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<Flat>> Get()
        {
            return await _flat
                .Include(f => f.Estate)
                .ToListAsync();
        }

    }
}
