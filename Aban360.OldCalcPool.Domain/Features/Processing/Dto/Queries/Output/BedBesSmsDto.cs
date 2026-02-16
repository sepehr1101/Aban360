namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record BedBesSmsDto
    {
        public string BillId { get; set; } = default!;
        public string PayId { get; set; } = default!;
        public string PreviousDateJalali { get; set; } = default!;
        public string CurrentDateJalali { get; set; }= default!;
        public string DateBed { get; set; } = default!;
        public string Deadline { get; set; } = default!;
        public int BranchTypeId { get; set; }
        public int PreviousNumber { get; set; }
        public int CurrentNumber { get; set; }
        public long Discount { get; set; }
        public long Payable { get; set; }
        public long SumCurrentItems { get; set; }
        public long SumAll { get; set; }
        public int CounterStateCode { get; set; }
    }
}
