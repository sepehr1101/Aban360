using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Implementations
{
    public class WaterMeterQueryService : IWaterMeterQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WaterMeter> _wateMere;
        public WaterMeterQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _wateMere = _uow.Set<WaterMeter>();
            _wateMere.NotNull(nameof(_wateMere));
        }

        public async Task<WaterMeter> Get(int id)
        {
            return await _uow.FindOrThrowAsync<WaterMeter>(id);
        }

        public async Task<ICollection<WaterMeter>> Get()
        {
            return await _wateMere.ToListAsync();
        }
    }
}
