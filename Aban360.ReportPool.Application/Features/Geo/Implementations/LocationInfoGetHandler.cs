using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Geo.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Geo;
using Aban360.ReportPool.Infrastructure.Features.Geo;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.Geo.Implementations
{
    internal class LocationInfoGetHandler : ILocationInfoGetHandler
    {
        private readonly ILocationInfoService _branchSpecificationSummaryInfoService;
        private readonly IGisService _gisService;
        public LocationInfoGetHandler(
            ILocationInfoService branchSpecificationSummaryInfoService,
            IGisService gisService)
        {
            _branchSpecificationSummaryInfoService = branchSpecificationSummaryInfoService;
            _branchSpecificationSummaryInfoService.NotNull(nameof(branchSpecificationSummaryInfoService));

            _gisService = gisService;
            _gisService.NotNull(nameof(gisService));
        }

        public async Task<LocationInfoDto> Handle(string billId, CancellationToken cancellationToken)
        {
            LocationInfoDto branchSpecificationSummaryInfo = await _branchSpecificationSummaryInfoService.GetInfo(billId);
            CustomerLocationDto customerLocation = await _gisService.GetCustomerLocation(new CustomerLocationInputDto(billId));
            LocationInfoDto result = GetLocationInfo(branchSpecificationSummaryInfo, customerLocation);

            return result;
        }

        private LocationInfoDto GetLocationInfo(LocationInfoDto locationInfo, CustomerLocationDto customerLocation)
        {
            locationInfo.X = customerLocation.X;
            locationInfo.Y = customerLocation.Y;

            var utm = UtmConverter.LatLonToUtm(double.Parse(customerLocation.X), double.Parse(customerLocation.Y));
            locationInfo.Easting = utm.Easting;
            locationInfo.Northing = utm.Northing;
            locationInfo.UtmZone = utm.Zone;
            locationInfo.Letter = utm.Letter;

            return locationInfo;
        }
    }
}
