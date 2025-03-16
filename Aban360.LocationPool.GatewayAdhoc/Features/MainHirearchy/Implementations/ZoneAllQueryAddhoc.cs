using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;

namespace Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Implementations
{
    internal sealed class ZoneAllQueryAddhoc : IZoneAllQueryAddhoc
    {
        private readonly IZoneGetAllHandler _zoneGetAllHandler;
        public ZoneAllQueryAddhoc(IZoneGetAllHandler zoneGetAllHandler)
        {
            _zoneGetAllHandler = zoneGetAllHandler;
            _zoneGetAllHandler.NotNull(nameof(zoneGetAllHandler));
        }

        public async Task<ICollection<NumericDictionary>> Get(CancellationToken cancellationToken)
        {
            ICollection<ZoneGetDto> zone = await _zoneGetAllHandler.Handle(cancellationToken);
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
