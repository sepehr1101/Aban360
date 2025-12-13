namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerInfoWithSameMobileNumberDataOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string BillId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public int DeletionStateId { get; set; }
        public string DeletionStateTitle { get; set; }
    }
}
