using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts
{
    public interface IConsumptionAverageManagementDetailHandler
    {
        Task<ReportOutput<ConsumptionAverageManagementHeaderOutputDto, ConsumptionAverageManagementSummaryOutputDto>> Handle(ConsumptionAverageManagementSummrayInputDto input, CancellationToken cancellationToken);
    }
}
