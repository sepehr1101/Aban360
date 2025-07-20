using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class ZoneAllHandler : IZoneAllHandler
    {
        private readonly IZoneQueryService _zoneQueryService;
        public ZoneAllHandler(IZoneQueryService zoneQueryService)
        {
            _zoneQueryService = zoneQueryService;
            _zoneQueryService.NotNull(nameof(zoneQueryService));
        }

        public async Task<IEnumerable<UserZoneIdsOutputDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<UserZoneIdsOutputDto> zone = await _zoneQueryService.Get();
            return zone;
        }
    }
}
