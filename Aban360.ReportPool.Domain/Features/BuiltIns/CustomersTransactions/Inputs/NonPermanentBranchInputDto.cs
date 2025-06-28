namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record NonPermanentBranchInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public ICollection<int> ZoneIds { get; set; }
    }
}
