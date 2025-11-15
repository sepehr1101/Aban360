namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record DebtorAndCreaditorOutputDto
    {
        public long? DebtAmount { get; set; }
        public long CreditAmount { get; set; }
        public int TypeCode { get; set; }
        public string RegisterDate { get; set; }
    }
}
