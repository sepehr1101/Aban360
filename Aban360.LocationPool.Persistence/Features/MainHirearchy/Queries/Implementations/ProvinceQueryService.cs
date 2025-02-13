using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Implementations
{
    public class ProvinceQueryService : IProvinceQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Province> _province;
        public ProvinceQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _province = _uow.Set<Province>();
            _province.NotNull(nameof(_province));
        }

        public async Task<Province> Get(short id)
        {
            return await _province
                .Include(p => p.Country)
                .Include(p=>p.CordinalDirection)
                .Where(p => p.Id == id)
                .SingleAsync();
        }

        public async Task<ICollection<Province>> Get()
        {
            return await _province
                .Include(p=>p.Country)
                .Include(p => p.CordinalDirection)
                .ToListAsync();
        }
    }
}
