using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Repositories
{
    internal sealed class RequestWaterMeterTagRepository : IRequestWaterMeterTagCommandService, IRequestWaterMeterTagQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestWaterMeterTag> _requestWaterMeterTag;
        public RequestWaterMeterTagRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestWaterMeterTag = _uow.Set<RequestWaterMeterTag>();
            _requestWaterMeterTag.NotNull(nameof(_requestWaterMeterTag));
        }

        public async Task Add(RequestWaterMeterTag requestWaterMeterTag)
        {
            await _requestWaterMeterTag.AddAsync(requestWaterMeterTag);
        }

        public async Task Remove(RequestWaterMeterTag requestWaterMeterTag)
        {
            _requestWaterMeterTag.Remove(requestWaterMeterTag);
        }
        public void Remove(ICollection<RequestWaterMeterTag> requestWaterMeterTags)
        {
            _requestWaterMeterTag.RemoveRange(requestWaterMeterTags);
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
