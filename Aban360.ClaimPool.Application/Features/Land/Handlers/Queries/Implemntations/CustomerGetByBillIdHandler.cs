using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Infrastructure.Features.Geo;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class CustomerGetByBillIdHandler : ICustomerGetByBillIdHandler
    {
        private readonly ISubscriptionQueryService _subscriptionAssignmentQueryService;
        private readonly IGisService _gisService;
        public CustomerGetByBillIdHandler(
            ISubscriptionQueryService subscriptionAssignmentQueryService,
            IGisService gisService)
        {
            _subscriptionAssignmentQueryService = subscriptionAssignmentQueryService;
            _subscriptionAssignmentQueryService.NotNull(nameof(subscriptionAssignmentQueryService));

            _gisService = gisService;
            _gisService.NotNull(nameof(gisService));
        }

        public async Task<SubscriptionGetDto> Handle(SearchInput inputDto, CancellationToken cancellationToken)
        {
            SubscriptionGetDto customerInfo = await _subscriptionAssignmentQueryService.GetInfo(inputDto.Input);
            if (customerInfo == null)
            {
                throw new BaseException("شناسه قبض یافت نشد");
            }
            CustomerLocationDto customerLocation = await _gisService.GetCustomerLocation(new CustomerLocationInputDto(inputDto.Input));
            return GetLocationInfo(customerInfo, customerLocation);
        }
        private SubscriptionGetDto GetLocationInfo(SubscriptionGetDto customerInfo, CustomerLocationDto customerLocation)
        {
            customerInfo.X = customerInfo.X;
            customerInfo.Y = customerInfo.Y;

            if (!string.IsNullOrWhiteSpace(customerInfo.X) && !string.IsNullOrWhiteSpace(customerInfo.Y))
            {
                var utm = UtmConverter.LatLonToUtm(double.Parse(customerInfo.X), double.Parse(customerInfo.Y));

                customerInfo.Easting = utm.Easting;
                customerInfo.Northing = utm.Northing;
                customerInfo.UtmZone = utm.Zone;
                customerInfo.Letter = utm.Letter;
            }

            return customerInfo;
        }
    }
}
