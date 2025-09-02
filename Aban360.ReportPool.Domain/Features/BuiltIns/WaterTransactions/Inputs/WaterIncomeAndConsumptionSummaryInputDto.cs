using Aban360.ReportPool.Domain.Constants;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record WaterIncomeAndConsumptionSummaryInputDto
    {
        public WaterIncomeAndConsumptionDetailInputDto Inputs { get; set; }
        public WaterIncomeAndConsumptionSummaryEnum EnumInput { get; set; }
    }
}
