using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Services;
public interface IVillageLookupService
{
    Task<IEnumerable<VillageGetDto>> VillageGetAllAsync(CancellationToken cancellationToken);
}

internal sealed class VillageLookupService : IVillageLookupService
{
    public VillageLookupService(IVillageQueryService villageQueryService)
    {
        _villageQueryService = villageQueryService;
        _villageQueryService.NotNull(nameof(villageQueryService));
    }

    private readonly IVillageQueryService _villageQueryService;
    public async Task<IEnumerable<VillageGetDto>> VillageGetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<VillageGetDto> villages = await _villageQueryService.Get();
        return villages;
    }
}
