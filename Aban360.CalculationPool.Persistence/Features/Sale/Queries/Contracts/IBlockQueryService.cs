using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts
{
    public interface IBlockQueryService
    {
        Task<IEnumerable<BlockGetDto>> Get();
    }
}
