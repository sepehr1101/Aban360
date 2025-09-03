namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ContractualAndOlgooLevelInputDto
    {
        public WaterIncomeAndConsumptionDetailInputDto Inputs { get; set; }
        public ICollection<ContractualAndOlgooLevelValuesInputDto> Values { get; set; }
        public bool IsConsumption { get; set; }
    }
}
