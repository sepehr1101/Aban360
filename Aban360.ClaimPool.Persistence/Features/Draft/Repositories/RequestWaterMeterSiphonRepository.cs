using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Repositories
{
    internal sealed class RequestWaterMeterSiphonRepository : IRequestWaterMeterSiphonCommandService, IRequestWaterMeterSiphonQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestWaterMeterSiphon> _requestWaterMeterSiphon;
        public RequestWaterMeterSiphonRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestWaterMeterSiphon = _uow.Set<RequestWaterMeterSiphon>();
            _requestWaterMeterSiphon.NotNull(nameof(_requestWaterMeterSiphon));
        }

        public async Task Add(RequestWaterMeterSiphon requestWaterMeterSiphon)
        {
            await _requestWaterMeterSiphon.AddAsync(requestWaterMeterSiphon);
        }

        public void Remove(RequestWaterMeterSiphon requestWaterMeterSiphon)
        {
            _requestWaterMeterSiphon.Remove(requestWaterMeterSiphon);
        }

        public async Task<RequestWaterMeterSiphon> Get(int id)
        {
            return await _uow.FindOrThrowAsync<RequestWaterMeterSiphon>(id);
        }
        
        public async Task<RequestWaterMeterSiphon> GetBySiphonId(int id)
        {
            return await _requestWaterMeterSiphon
                .Where(r => r.SiphonId == id)
                .SingleOrDefaultAsync();
        }

        public async Task<ICollection<RequestWaterMeterSiphon>> GetByWaterMeterId(int waterMeterId)
        {
            return await _requestWaterMeterSiphon
                .Include(r => r.RequestSiphon)
                .Where(r => r.WaterMeterId == waterMeterId)
                .ToListAsync();
        }
        
        public async Task<ICollection<RequestWaterMeterSiphon>> Get()
        {
            return await _requestWaterMeterSiphon.ToListAsync();
        }

    }
}
