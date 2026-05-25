namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record ServiceLinkRegisterItemDto
    {
        public int BankCode { get; set; }
        public string BankBranchCode { get; set; }
        public string BankDateJalali { get; set; }
        public string PayDateJalali { get; set; }
        public long Cod1  { get; set; }
        public long Cod2  { get; set; }
        public long Cod3  { get; set; }
        public string ChequeOrMinutesNumber { get; set; }
    }
}