using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Geo.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Infrastructure.Features.Geo;

namespace Aban360.ReportPool.Application.Features.Geo.Implementations
{
    internal sealed class LocationFileHandler : ILocationFileHandler
    {
        private readonly ICommonZoneService _zoneService;
        private readonly ICommonMemberQueryService _memberQueryService;
        private readonly IGisService _gisService;
        private readonly IMapService _mapService;
        public LocationFileHandler(
            ICommonZoneService zoneService,
            ICommonMemberQueryService memberQueryService,
            IGisService gisService,
            IMapService mapService)
        {
            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));

            _memberQueryService = memberQueryService;
            _memberQueryService.NotNull(nameof(memberQueryService));

            _gisService = gisService;
            _gisService.NotNull(nameof(gisService));

            _mapService = mapService;
            _mapService.NotNull(nameof(mapService));
        }

        public async Task<byte[]> Handle(string billId, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _memberQueryService.Get(billId);
            await _zoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);

            CustomerLocationDto locationInfo = await _gisService.GetCustomerLocation(new CustomerLocationInputDto(billId));
            string base64Image = await _mapService.GenerateMapBase64(locationInfo.X, locationInfo.Y);
            byte[] bytes = Convert.FromBase64String(base64Image);
            return bytes;
        }
    }
}
