using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Infrastructure.Features.Geo;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
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
            var customerLocation = await _gisService.GetCustomerLocation(new CustomerLocationInputDto(billId));
            branchSpecificationSummaryInfo.X = customerLocation.X;
            branchSpecificationSummaryInfo.Y = customerLocation.Y;
            var utm= UtmConverter.LatLonToUtm(double.Parse(customerLocation.X), double.Parse(customerLocation.Y));
            branchSpecificationSummaryInfo.Easting = utm.Easting;
            branchSpecificationSummaryInfo.Northing = utm.Northing;
            branchSpecificationSummaryInfo.UtmZone = utm.Zone;
            branchSpecificationSummaryInfo.Letter = utm.Letter;

            return branchSpecificationSummaryInfo;
        }
    }
}
