using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.ApplicationUser;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts
{
    public interface ICalculationRequestInsertManualHandler
    {
        Task<SaleAndAfterSaleDataOutputDto> Handle(KartInsertManualInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
