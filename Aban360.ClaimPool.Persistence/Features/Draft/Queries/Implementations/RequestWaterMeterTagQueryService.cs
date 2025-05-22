using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Implementations
{
    internal sealed class RequestWaterMeterTagQueryService : IRequestWaterMeterTagQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestWaterMeterTag> _requestWaterMeterTag;
        public RequestWaterMeterTagQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestWaterMeterTag = _uow.Set<RequestWaterMeterTag>();
            _requestWaterMeterTag.NotNull(nameof(_requestWaterMeterTag));
        }

        public async Task<RequestWaterMeterTag> Get(int id)
        {
            return await _uow.FindOrThrowAsync<RequestWaterMeterTag>(id);
        }
        
        public async Task<ICollection<RequestWaterMeterTag>> GetByWaterMeterId(int id)
        {
            return await _requestWaterMeterTag
                .Where(r => r.WaterMeterId == id)
                .ToListAsync();
        }

        public async Task<ICollection<RequestWaterMeterTag>> Get()
        {
            return await _requestWaterMeterTag.ToListAsync();
        }
    }
}
