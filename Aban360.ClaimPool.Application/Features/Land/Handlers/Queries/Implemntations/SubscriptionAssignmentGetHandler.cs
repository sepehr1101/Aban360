using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Infrastructure.Features.Geo;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class SubscriptionAssignmentGetHandler : ISubscriptionAssignmentGetHandler
    {
        private readonly ISubscriptionQueryService _subscriptionAssignmentQueryService;
        private readonly IGisService _gisService;
        public SubscriptionAssignmentGetHandler(
            ISubscriptionQueryService subscriptionAssignmentQueryService
            , IGisService gisService)
        {
            _subscriptionAssignmentQueryService = subscriptionAssignmentQueryService;
            _subscriptionAssignmentQueryService.NotNull(nameof(subscriptionAssignmentQueryService));

            _gisService = gisService;
            _gisService.NotNull(nameof(gisService));
        }

        public async Task<SubscriptionAssignmentGetDto> Handle(string input, CancellationToken cancellationToken)
        {
            SubscriptionAssignmentGetDto subscriptionAssignment = await _subscriptionAssignmentQueryService.Get(input);
            CustomerLocationDto customerLocation = await _gisService.GetCustomerLocation(new CustomerLocationInputDto(input));
            SubscriptionAssignmentGetDto result = GetLocationInfo(subscriptionAssignment, customerLocation);
            return result;
        }
        private SubscriptionAssignmentGetDto GetLocationInfo(SubscriptionAssignmentGetDto input, CustomerLocationDto customerLocation)
        {
            input.X = customerLocation.X;
            input.Y = customerLocation.Y;

            input.Easting = 0;
            input.Northing = 0;
            input.UtmZone = 0;
            input.Letter = string.Empty;

            if (!string.IsNullOrWhiteSpace(input.X) && !string.IsNullOrWhiteSpace(input.Y))
            {
                var utm = UtmConverter.LatLonToUtm(double.Parse(input.X), double.Parse(input.Y));

                input.Easting = utm.Easting;
                input.Northing = utm.Northing;
                input.UtmZone = utm.Zone;
                input.Letter = utm.Letter;
            }

            return input;
        }
    }
}
