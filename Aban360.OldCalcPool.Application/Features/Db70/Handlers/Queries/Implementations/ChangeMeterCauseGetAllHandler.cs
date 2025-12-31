using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Implementations
{
    internal sealed class ChangeMeterCauseGetAllHandler : IChangeMeterCauseGetAllHandler
    {
        private readonly IChangeMeterCauseQueryService _changeMeterCauseQueryService;

        public ChangeMeterCauseGetAllHandler(IChangeMeterCauseQueryService changeMeterCauseQueryService)
        {
            _changeMeterCauseQueryService = changeMeterCauseQueryService;
            _changeMeterCauseQueryService.NotNull(nameof(changeMeterCauseQueryService));
        }
        public async Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _changeMeterCauseQueryService.Get();
            return result;
        }
    }
}
