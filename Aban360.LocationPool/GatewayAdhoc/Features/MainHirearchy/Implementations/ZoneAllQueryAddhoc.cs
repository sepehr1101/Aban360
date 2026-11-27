using Aban360.LocationPool.Application.Features.MainHierarchy.Services;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Implementations
{
    internal sealed class ZoneAllQueryAddhoc : IZoneAllQueryAddhoc
    {
        private readonly IZoneService _service;
        public ZoneAllQueryAddhoc(IZoneService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        public async Task<ICollection<NumericDictionary>> Get(CancellationToken cancellationToken)
        {
            ICollection<ZoneGetDto> zone = await _service.GetAllAsync(cancellationToken);
            return zone
                .Select(x => new NumericDictionary()
                {
                    Id = x.Id,
                    Title = x.Title,
                })
                .ToList();
        }
    }
}
