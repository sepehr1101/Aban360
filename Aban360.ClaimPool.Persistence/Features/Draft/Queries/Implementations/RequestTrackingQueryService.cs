using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Implementations
{
    internal sealed class RequestTrackingQueryService : IRequestTrackingQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestTracking> _requestTracking;
        public RequestTrackingQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestTracking = _uow.Set<RequestTracking>();
            _requestTracking.NotNull(nameof(_requestTracking));
        }

        public async Task<RequestTracking> Get(int id)
        {
            return await _uow.FindOrThrowAsync<RequestTracking>(id);
        }
        
        public async Task<ICollection<RequestTracking>> GetByWaterMeterId(int id)
        {
            return await _requestTracking
                .Where(tracking => tracking.WaterMeterId == id)
                .ToListAsync();
        }

        public async Task<ICollection<RequestTracking>> Get()
        {
            return await _requestTracking.ToListAsync();
        }
    }
}