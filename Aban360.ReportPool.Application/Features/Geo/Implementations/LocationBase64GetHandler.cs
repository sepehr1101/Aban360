using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Geo.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Geo;
using Aban360.ReportPool.Infrastructure.Features.Geo;
using NetTopologySuite.Geometries;

namespace Aban360.ReportPool.Application.Features.Geo.Implementations
{
    internal sealed class LocationBase64GetHandler : ILocationBase64GetHandler
    {
        private readonly ICommonZoneService _zoneService;
        private readonly ICommonMemberQueryService _memberQueryService;
        private readonly IGisService _gisService;
        private readonly IMapService _mapService;
        public LocationBase64GetHandler(
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

        public async Task<LocationBase64Dto> Handle(string billId, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _memberQueryService.Get(billId);
            await _zoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);

            CustomerLocationDto locationInfo = await _gisService.GetCustomerLocation(new CustomerLocationInputDto(billId));
            string base64Image = string.Empty;
            
            if (locationInfo is null ||
                string.IsNullOrWhiteSpace(locationInfo.X) || locationInfo.X.Trim() == "0" ||
                string.IsNullOrWhiteSpace(locationInfo.Y) || locationInfo.Y.Trim() == "0")
            {
                base64Image = await Base64Operation.GetNotFoundBase64(cancellationToken);
            }
            base64Image = await _mapService.GenerateMapBase64(locationInfo.X, locationInfo.Y);
            return new LocationBase64Dto(zoneIdAndCustomerNumber.CustomerNumber, zoneIdAndCustomerNumber.ZoneId, billId, base64Image);
        }
    }
}
