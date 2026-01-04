using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Implementations
{
    internal sealed class OfferingGetOfferingHandler : IOfferingGetOfferingHandler
    {
        private readonly IOfferingQueryService _offeringQueryService;

        public OfferingGetOfferingHandler(IOfferingQueryService offeringQueryService)
        {
            _offeringQueryService = offeringQueryService;
            _offeringQueryService.NotNull(nameof(offeringQueryService));
        }
        public async Task<NumericDictionary> Handle(SearchByIdInput input, CancellationToken cancellationToken)
        {
            NumericDictionary result = await _offeringQueryService.Get(input);
            return result;
        }
    }
}
