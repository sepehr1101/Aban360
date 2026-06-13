using Aban360.Common.Exceptions;
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
        private readonly ILocationInfoService _locationInfoService;
        private readonly IGisService _gisService;
        public LocationInfoGetHandler(
            ILocationInfoService locationInfoService,
            IGisService gisService)
        {
            _locationInfoService = locationInfoService;
            _locationInfoService.NotNull(nameof(locationInfoService));

            _gisService = gisService;
            _gisService.NotNull(nameof(gisService));
        }

        public async Task<LocationInfoDto> Handle(string billId, CancellationToken cancellationToken)
        {
            try
            {
                int timeoutSecond = 6;
                LocationInfoDto locationInfo = await _locationInfoService.GetInfo(billId);
                CustomerLocationDto customerLocation = await _gisService.GetCustomerLocation(new CustomerLocationInputDto(billId), timeoutSecond);
                LocationInfoDto result = GetLocationInfo(locationInfo, customerLocation);

                return result;
            }
            catch (OperationCanceledException)
            {
                throw new BaseException("سرویس gis در دسترس نیست. لطفا با پشتیبانی موضوع را مطرح بفرمایید");
            }
            catch (Exception ex)
            {
                throw new BaseException("سرویس gis در دسترس نیست. لطفا با پشتیبانی موضوع را مطرح بفرمایید");
            }
         
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
