using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts
{
    public interface IBillInstallmentManualCreateHandler
    {
        Task<ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentDataOutputDto>> Handle(BillInstallmentManualInputDto input, IAppUser appUser, CancellationToken cancellationToken);
    }
}
