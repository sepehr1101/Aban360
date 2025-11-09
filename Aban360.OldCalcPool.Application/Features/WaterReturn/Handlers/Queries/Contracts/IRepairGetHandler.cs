using Aban360.Common.BaseEntities;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Queries;

namespace Aban360.OldCalcPools.Application.Features.WaterReturn.Handlers.Queries.Contracts
{
    public interface IRepairGetHandler
    {
        Task<RepairGetDto> Handle(SearchInput input, CancellationToken cancellationToken);
    }
}
