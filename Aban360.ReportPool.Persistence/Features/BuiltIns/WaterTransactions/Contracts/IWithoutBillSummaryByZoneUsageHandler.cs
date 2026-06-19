using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts
{
    public interface IWithoutBillSummaryByZoneUsageHandler
    {
        Task<ReportOutput<WithoutBillHeaderOutputDto, ReportOutput<WithoutBillSummaryDataOutputDto, WithoutBillSummaryDataOutputDto>>> Handle(WithoutBillInputDto input, CancellationToken cancellationToken);
        Task<ReportOutput<WithoutBillHeaderOutputDto, WithoutBillSummaryDataOutputDto>> HandleFlat(WithoutBillInputDto input, CancellationToken cancellationToken);
    }
}
