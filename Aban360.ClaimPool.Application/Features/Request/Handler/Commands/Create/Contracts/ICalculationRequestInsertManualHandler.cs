using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts
{
    public interface ICalculationRequestInsertManualHandler
    {
        Task<SaleAndAfterSaleDataOutputDto> Handle(KartInsertManualInputDto inputDto, int userCode, CancellationToken cancellationToken);
    }
}
