using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Implementations
{
    internal sealed class RequestWaterMeterQueryService : IRequestWaterMeterQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestWaterMeter> _requestWaterMeter;
        public RequestWaterMeterQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestWaterMeter = _uow.Set<RequestWaterMeter>();
            _requestWaterMeter.NotNull(nameof(_requestWaterMeter));
        }

        public async Task<RequestWaterMeter> Get(int id)
        {
            return await _uow.FindOrThrowAsync<RequestWaterMeter>(id);
        }

        public async Task<ICollection<RequestWaterMeter>> Get()
        {
            return await _requestWaterMeter.ToListAsync();
        }
    }
}
