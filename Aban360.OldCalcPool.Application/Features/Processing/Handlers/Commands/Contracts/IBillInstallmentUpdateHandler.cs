using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.ApplicationUser;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts
{
    public interface IBillInstallmentUpdateHandler
    {
        Task<BillInstallmentDataOutputDto> Handle(BillInstallmentUpdateInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
