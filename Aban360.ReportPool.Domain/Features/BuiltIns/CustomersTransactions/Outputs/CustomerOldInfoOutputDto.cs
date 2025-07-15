namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerOldInfoOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public int OldCustomerNumber { get; set; }
        public string BillId { get; set; }
        public string OldBillId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }

    }
}