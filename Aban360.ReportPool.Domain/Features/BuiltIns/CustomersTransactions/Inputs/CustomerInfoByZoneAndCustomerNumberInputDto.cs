namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record CustomerInfoByZoneAndCustomerNumberInputDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
    }
}
