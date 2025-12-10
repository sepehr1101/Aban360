namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record QuarterDto
    {
        public int LessThanContractualOrOlgo { get; set; }
        public int Between1_2ContractualOrOlgo { get; set; }
        public int Between2_3ContractualOrOlgo { get; set; }
        public int MoreThanContractualOrOlgo { get; set; }
        public QuarterDto(int lessThanContractualOrOlgo, int between1_2ContractualOrOlgo, int between2_3ContractualOrOlgo, int moreThanContractualOrOlgo)
        {
            LessThanContractualOrOlgo = lessThanContractualOrOlgo;
            Between1_2ContractualOrOlgo = between1_2ContractualOrOlgo;
            Between2_3ContractualOrOlgo = between2_3ContractualOrOlgo;
            MoreThanContractualOrOlgo = moreThanContractualOrOlgo;
        }
    }
}
