using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Metering.Queries.Implementations
{
    public class MeterProducerQueryService : IMeterProducerQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<MeterProducer> _meterProducer;
        public MeterProducerQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _meterProducer = _uow.Set<MeterProducer>();
            _meterProducer.NotNull(nameof(_meterProducer));
        }

        public async Task<MeterProducer> Get(short id)
        {
            return await _uow.FindOrThrowAsync<MeterProducer>(id);
        }

        public async Task<ICollection<MeterProducer>> Get()
        {
            return await _meterProducer.ToListAsync();
        }
    }
}
