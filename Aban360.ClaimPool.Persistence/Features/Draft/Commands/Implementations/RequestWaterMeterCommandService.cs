using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Commands.Implementations
{
    internal sealed class RequestWaterMeterCommandService : IRequestWaterMeterCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<RequestWaterMeter> _requestWaterMeter;
        public RequestWaterMeterCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _requestWaterMeter = _uow.Set<RequestWaterMeter>();
            _requestWaterMeter.NotNull(nameof(_requestWaterMeter));
        }

        public async Task Add(RequestWaterMeter requestWaterMeter)
        {
            await _requestWaterMeter.AddAsync(requestWaterMeter);
        }

        public async Task Remove(RequestWaterMeter requestWaterMeter)
        {
            _requestWaterMeter.Remove(requestWaterMeter);
        }
    }
}
