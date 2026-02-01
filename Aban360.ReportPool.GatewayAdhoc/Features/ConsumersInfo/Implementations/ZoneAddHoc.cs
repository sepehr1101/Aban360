using Aban360.Common.Extensions;
using Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Contracts;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;

namespace Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Implementations
{
    internal sealed class ZoneAddHoc : IZoneAddHoc
    {
        private readonly IZoneQueryService _zoneQueryService;
        public ZoneAddHoc(IZoneQueryService zoneQueryService)
        {
            _zoneQueryService = zoneQueryService;
            _zoneQueryService.NotNull(nameof(zoneQueryService));
        }

        public async Task<bool> GetArticle2(int zoneId)
        {
            return await _zoneQueryService.GetArticle2(zoneId);
        }
    }
}
