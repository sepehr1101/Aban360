using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Implementations
{
    internal sealed class WaterMeterChangeNumberHistoryQueryService : IWaterMeterChangeNumberHistoryQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WaterMeterChangeNumberHistory> _readingPeriodType;
        public WaterMeterChangeNumberHistoryQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _readingPeriodType = _uow.Set<WaterMeterChangeNumberHistory>();
            _readingPeriodType.NotNull(nameof(_readingPeriodType));
        }

        public async Task<WaterMeterChangeNumberHistory> Get(long id)
        {
            return await _uow.FindOrThrowAsync<WaterMeterChangeNumberHistory>(id);
        }

        public async Task<ICollection<WaterMeterChangeNumberHistory>> Get()
        {
            return await _readingPeriodType.ToListAsync();
        }
    }
}
