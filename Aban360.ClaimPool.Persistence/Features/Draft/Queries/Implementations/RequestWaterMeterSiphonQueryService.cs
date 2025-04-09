using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Implementations
{
    internal sealed class RequestWaterMeterSiphonQueryService : IRequestWaterMeterSiphonQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestWaterMeterSiphon> _requestWaterMeterSiphon;
        public RequestWaterMeterSiphonQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestWaterMeterSiphon = _uow.Set<RequestWaterMeterSiphon>();
            _requestWaterMeterSiphon.NotNull(nameof(_requestWaterMeterSiphon));
        }

        public async Task<RequestWaterMeterSiphon> Get(int id)
        {
            return await _uow.FindOrThrowAsync<RequestWaterMeterSiphon>(id);
        }

        public async Task<ICollection<RequestWaterMeterSiphon>> Get()
        {
            return await _requestWaterMeterSiphon.ToListAsync();
        }
    }
}
