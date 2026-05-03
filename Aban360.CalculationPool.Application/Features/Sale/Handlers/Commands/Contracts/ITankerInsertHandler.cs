using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts
{
    public interface ITankerInsertHandler
    {
        Task<TankerWaterCalculationOutputDto> Handle(TankerInsertInputDto inputDto, int userCode, CancellationToken cancellationToken);
    }
}
