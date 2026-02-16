using Aban360.Common.Extensions;
using static Aban360.Common.Timing.CalculationDistanceDate;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Application.Features.Geo.Contracts;
using Aban360.ReportPool.Domain.Features.Geo;
using Aban360.ReportPool.Infrastructure.Features.Geo;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal class CustomerGeneralInfoGetHandler : ICustomerGeneralInfoGetHandler
    {
        private readonly ICustomerGeneralInfoQueryService _customerGeneralInfoService;
        private readonly ICustomerInfoQueryService _customerInfoQueryService;
        private readonly ILocationInfoGetHandler _locationInfoService;
        public CustomerGeneralInfoGetHandler(
            ICustomerGeneralInfoQueryService customerGeneralInfoService,
            ICustomerInfoQueryService customerInfoQueryService,
            ILocationInfoGetHandler locationInfoService)
        {
            _customerGeneralInfoService = customerGeneralInfoService;
            _customerGeneralInfoService.NotNull(nameof(customerGeneralInfoService));

            _customerInfoQueryService = customerInfoQueryService;
            _customerInfoQueryService.NotNull(nameof(customerInfoQueryService));

            _locationInfoService = locationInfoService;
            _locationInfoService.NotNull(nameof(locationInfoService));
        }

        public async Task<ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto>> Handle(SearchInput input, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumberOutputDto zoneIdAndCustomerNumber = await _customerInfoQueryService.GetZoneIdAndCustomerNumber(input.Input);

            ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> result = await _customerGeneralInfoService.Get(zoneIdAndCustomerNumber);
            result.ReportData.FirstOrDefault().MeterLife = GetMeterLife(result.ReportData.FirstOrDefault(), result.ReportHeader);

            var (e, n) = await GetLocation(input.Input,cancellationToken);
            result.ReportData.FirstOrDefault().E = e;
            result.ReportData.FirstOrDefault().N= n;

            return result;
        }
        private string GetMeterLife(CustomerGeneralInfoDataDto data, CustomerGeneralInfoHeaderDto header)
        {
            if (string.IsNullOrWhiteSpace(data.MeterChangeDateJalali) || data.MeterChangeDateJalali.CompareTo(header.WaterInstallationDateJalali) <= 0)
            {
                CalcDistanceResultDto distance = CalcDistance(header.WaterInstallationDateJalali);
                if (!distance.HasError)
                {
                    return distance.DistanceText;
                }
            }
            else
            {
                CalcDistanceResultDto distance = CalcDistance(data.MeterChangeDateJalali);
                if (!distance.HasError)
                {
                    return distance.DistanceText;
                }
            }

            return string.Empty;
        }
        private async Task<(double, double)> GetLocation(string billId, CancellationToken cancellationToken)
        {
            LocationInfoDto location = await _locationInfoService.Handle(billId, cancellationToken);
            var utm=UtmConverter.LatLonToUtm(double.Parse(location.X), double.Parse(location.Y));

            return (utm.Easting,utm.Northing);
        }
    }
}