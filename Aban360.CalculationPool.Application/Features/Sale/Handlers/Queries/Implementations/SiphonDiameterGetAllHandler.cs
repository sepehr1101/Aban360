using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations
{
    internal sealed class SiphonDiameterGetAllHandler : ISiphonDiameterGetAllHandler
    {
        private readonly ISiphonDiameterQueryService _queryService;
        public SiphonDiameterGetAllHandler(ISiphonDiameterQueryService queryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _queryService.Get();
            return result;
        }
    }
}
