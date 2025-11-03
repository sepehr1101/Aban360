using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;

namespace Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts
{
    public interface IVirtualCategoryQueryService
    {
        Task<VirtualCategoryGetDto> Get(VirtualCategorySearchInputDto input);
        Task<IEnumerable<VirtualCategoryGetDto>> Get();
    }
}
