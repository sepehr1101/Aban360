using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Implementations
{
    internal sealed class OfferingGetAllHandler : IOfferingGetAllHandler
    {
        private readonly IOfferingQueryService _offeringQueryService;

        public OfferingGetAllHandler(IOfferingQueryService offeringQueryService)
        {
            _offeringQueryService = offeringQueryService;
            _offeringQueryService.NotNull(nameof(offeringQueryService));
        }
        public async Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _offeringQueryService.Get();
            return result;
        }
    }
}
