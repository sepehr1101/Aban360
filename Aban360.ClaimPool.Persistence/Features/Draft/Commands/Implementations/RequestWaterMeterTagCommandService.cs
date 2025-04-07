using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Implementations
{
    internal sealed class RequestWaterMeterTagCommandService : IRequestWaterMeterTagCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestWaterMeterTag> _requestWaterMeterTag;
        public RequestWaterMeterTagCommandService(IUnitOfWork uow)
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
    }
}
