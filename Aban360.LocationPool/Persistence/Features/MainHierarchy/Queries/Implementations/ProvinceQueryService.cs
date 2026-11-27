using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Implementations
{
    internal sealed class ProvinceQueryService : IProvinceQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Province> _provinces;
        public ProvinceQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _provinces = _uow.Set<Province>();
            _provinces.NotNull(nameof(_provinces));
        }

        public async Task<Province> Get(short id)
        {
            return await _provinces
                .Include(p => p.Country)
                .Include(p=>p.CordinalDirection)
                .Where(p => p.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<Province>> Get()
        {
            return await _provinces
                .Include(p=>p.Country)
                .Include(p => p.CordinalDirection)
                .ToListAsync();
        }
    }
}
