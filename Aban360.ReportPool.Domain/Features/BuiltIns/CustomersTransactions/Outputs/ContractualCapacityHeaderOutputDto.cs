namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record ContractualCapacityHeaderOutputDto
    {
        public int FromContractualCapacity { get; set; }
        public int ToContractualCapacity { get; set; }

        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public string ReportDate { get; set; } = default!;
        public int RecordCount { get; set; }
    }
}
