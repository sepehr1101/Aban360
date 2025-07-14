namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record CustomerOldInfoInputDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
    }
}