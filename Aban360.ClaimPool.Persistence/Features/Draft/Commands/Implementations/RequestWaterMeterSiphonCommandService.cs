using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Implementations
{
    internal sealed class RequestWaterMeterSiphonCommandService : IRequestWaterMeterSiphonCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestWaterMeterSiphon> _requestWaterMeterSiphon;
        public RequestWaterMeterSiphonCommandService(IUnitOfWork uow)
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

        public async Task Remove(RequestWaterMeterSiphon requestWaterMeterSiphon)
        {
            _requestWaterMeterSiphon.Remove(requestWaterMeterSiphon);
        }
    }
}
