using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class VillageGetAllHandler : IVillageGetAllHandler
    {
        private readonly IVillageQueryService _villageQueryService;
        public VillageGetAllHandler(IVillageQueryService villageQueryService)
        {
            _villageQueryService = villageQueryService;
            _villageQueryService.NotNull(nameof(villageQueryService));
        }

        public async Task<IEnumerable<VillageGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<VillageGetDto> villages = await _villageQueryService.Get();
            return villages;
        }
    }
}
