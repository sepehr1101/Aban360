namespace Aban360.ReportPool.Domain.Features.Transactions
{
    public record ZoneIdAndCustomerNumberOutputDto
    {
        public int ZoneId { get; set; }
        public string CustomerNumber { get; set; }
    }
}
