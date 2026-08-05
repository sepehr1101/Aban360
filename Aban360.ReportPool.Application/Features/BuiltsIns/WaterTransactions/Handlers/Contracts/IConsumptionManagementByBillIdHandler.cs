using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts
{
    public interface IConsumptionManagementByBillIdHandler
    {
        Task<FlatReportOutput<MemberInfoGetDto, CosnumptionManagementByBillIdDataOutputDto>> Handle(ConsumptionManagementByBillIdInputDto input, IAppUser appUser, CancellationToken cancellationToken);
    }
}
