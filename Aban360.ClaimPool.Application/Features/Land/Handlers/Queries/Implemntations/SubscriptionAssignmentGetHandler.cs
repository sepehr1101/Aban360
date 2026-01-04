using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Infrastructure.Features.Geo;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class SubscriptionAssignmentGetHandler : ISubscriptionAssignmentGetHandler
    {
        private readonly ISubscriptionAssignmentQueryService _subscriptionAssignmentQueryService;
        public SubscriptionAssignmentGetHandler(ISubscriptionAssignmentQueryService subscriptionAssignmentQueryService)
        {
            _subscriptionAssignmentQueryService = subscriptionAssignmentQueryService;
            _subscriptionAssignmentQueryService.NotNull(nameof(subscriptionAssignmentQueryService));
        }

        public async Task<SubscriptionAssignmentGetDto> Handle(string input, CancellationToken cancellationToken)
        {
            SubscriptionAssignmentGetDto subscriptionAssignment = await _subscriptionAssignmentQueryService.Get(input);
            SubscriptionAssignmentGetDto result = GetEN(subscriptionAssignment);
            return result;
        }
        private SubscriptionAssignmentGetDto GetEN(SubscriptionAssignmentGetDto input)
        {
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
