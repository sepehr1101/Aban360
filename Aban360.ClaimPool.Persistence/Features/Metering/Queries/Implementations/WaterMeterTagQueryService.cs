using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Implementations
{
    internal sealed class WaterMeterTagQueryService : IWaterMeterTagQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WaterMeterTag> _waterMeterTag;
        public WaterMeterTagQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _waterMeterTag = _uow.Set<WaterMeterTag>();
            _waterMeterTag.NotNull(nameof(_waterMeterTag));
        }

        public async Task<ICollection<WaterMeterTag>> Get(string billId)
        {
            return await _waterMeterTag
                 .Include(w => w.WaterMeter)
                 .Where(w => w.WaterMeter.BillId == billId)
                 .ToListAsync();
        }
        public async Task<WaterMeterTag> Get(int id)
        {
            return await _uow.FindOrThrowAsync<WaterMeterTag>(id);
        }
    }
}
