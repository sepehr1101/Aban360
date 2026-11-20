using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Services;
public interface IModifyCauseLookupService
{
    Task<IEnumerable<NumericDictionary>> ModifyCauseGetAllAsync(CancellationToken cancellationToken);
}

internal sealed class ModifyCauseLookupService : IModifyCauseLookupService
{
    public ModifyCauseLookupService(IModifyCauseQueryService modifyCauseQueryService)
    {
        _modifyCauseQueryService = modifyCauseQueryService;
        _modifyCauseQueryService.NotNull(nameof(modifyCauseQueryService));
    }

    private readonly IModifyCauseQueryService _modifyCauseQueryService;
    public async Task<IEnumerable<NumericDictionary>> ModifyCauseGetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<NumericDictionary> ModifyCauses = await _modifyCauseQueryService.Get();
        return ModifyCauses;
    }
}
