using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts
{
    public interface IServiceLinkInstallmentHandler
    {
        Task<ReportOutput<InstallmentHeaderOutputDto, InstallmentDataOutputDto>> Handle(ServiceLinkInstallmentInputDto inputDto, CancellationToken cancellationToken);
    }
}
