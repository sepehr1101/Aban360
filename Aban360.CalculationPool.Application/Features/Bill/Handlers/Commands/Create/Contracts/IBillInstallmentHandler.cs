using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts
{
    public interface IBillInstallmentHandler
    {
        Task<ReportOutput<InstallmentHeaderOutputDto, InstallmentDataOutputDto>> Handle(BillInstallmentInputDto inputDto, CancellationToken cancellationToken);
    }
}
