namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record ContractRepairInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public IEnumerable<int> ZoneIds { get; set; }
        public IEnumerable<int> UsageIds { get; set; }
        public bool IsWater { get; set; }

    }
}
