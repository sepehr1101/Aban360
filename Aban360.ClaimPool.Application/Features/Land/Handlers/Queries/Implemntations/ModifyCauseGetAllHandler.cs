using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class ModifyCauseGetAllHandler : IModifyCauseGetAllHandler
    {
        private readonly IModifyCauseQueryService _modifyCauseQueryService;
        public ModifyCauseGetAllHandler(IModifyCauseQueryService modifyCauseQueryService)
        {
            _modifyCauseQueryService = modifyCauseQueryService;
            _modifyCauseQueryService.NotNull(nameof(modifyCauseQueryService));
        }

        public async Task<IEnumerable<NumericDictionary>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> ModifyCauses = await _modifyCauseQueryService.Get();
            return ModifyCauses;
        }
    }
}
