using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts
{
    public interface IWaterDiscountQueryService
    {
        Task<ReportOutput<WaterDiscountDetailHeaderOutputDto, WaterDiscountDetailDataOutputDto>> GetDetail(WaterDiscountDetailInputDto input);
        Task<ReportOutput<WaterDiscountSummaryHeaderOutputDto, WaterDiscountSummaryDataOutputDto>> GetSummary(WaterDiscountSummaryInputDto input);
    }
}