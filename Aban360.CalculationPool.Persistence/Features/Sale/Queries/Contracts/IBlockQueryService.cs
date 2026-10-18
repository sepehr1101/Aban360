using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts
{
    public interface IBlockQueryService
    {
        Task<IEnumerable<BlockGetDto>> Get();
    }
}
