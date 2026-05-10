using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Implementations
{
    public interface ITankerQueryService
    {
        Task<ICollection<TankerWaterDateOutputDto>> Get(TankerWaterInputDto input);
    }//todo: Write Order Code
}
