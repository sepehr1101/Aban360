namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerOldInfoOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; } = default!;
        public int CustomerNumber { get; set; }
        public int OldCustomerNumber { get; set; }
        public string BillId { get; set; } = default!;
        public string? OldBillId { get; set; }
        public string FirstName { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string? VillageId { get; set; }
        public string? VillageName { get; set; }
        public int? RegionId { get; set; }
        public string? RegionTitle { get; set; }
    }   
}