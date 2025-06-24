namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record ContractualCapacityHeaderOutputDto
    {
        public string FromContractualCapacity { get; set; }
        public string ToContractualCapacity { get; set; }

        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public string ReportDate { get; set; }
        public int RecordCount { get; set; }
    }
}
