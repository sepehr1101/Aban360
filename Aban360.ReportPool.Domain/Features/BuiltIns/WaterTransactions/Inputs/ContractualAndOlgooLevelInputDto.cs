using Aban360.ReportPool.Domain.Constants;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ContractualAndOlgooLevelInputDto
    {
        public WaterIncomeAndConsumptionDetailInputDto? Inputs { get; set; }
        public ICollection<ContractualAndOlgooLevelValuesInputDto>? Values { get; set; }
        public ContractualAndOlgooLevelInputEnum TypeLevel {  get; set; }
    }
}
