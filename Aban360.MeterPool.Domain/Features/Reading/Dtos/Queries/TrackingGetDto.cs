namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record TrackingGetDto
    {
        public Guid Id { get; set; }
        public Guid BlockId { get; set; }
        public long TrackNumber { get; set; }
        public Guid? TbReadingProgramId { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; } = null!;
        public int FlowStepId { get; set; }
        public string FromReadingNumber { get; set; } = null!;
        public string ToReadingNumber { get; set; } = null!;
        public string? ListNumber { get; set; }
        public Guid InsertBy { get; set; }
        public string InsertDateJalali { get; set; } = null!;
        public DateTime InsertDateTime { get; set; }
        public bool Seen { get; set; }
        public int CounterReaderCode { get; set; }
        public Guid CounterReaderId { get; set; }
        public string CounterReaderName { get; set; } = null!;
        public string CounterReaderMobile { get; set; } = null!;
        public bool IsBazdid { get; set; }
        public bool? IsRoosta { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public int OverallCustomerCount { get; set; }
        public int ItemCustomerCount { get; set; }
        public int ProvisionallyPercent { get; set; }
        public int ImagePercent { get; set; }
        public bool HasPreNumber { get; set; }
        public bool DisplayBillId { get; set; }
        public bool DisplayCustomerNumber { get; set; }
        public string? Description { get; set; }
        public string? X { get; set; }
        public string? Y { get; set; }
        public bool? DisplayPreDate { get; set; }
        public bool? DisplayMobile { get; set; }
        public bool? HasImage { get; set; }
        public bool? DisplayDebt { get; set; }
        public bool? DisplayIcons { get; set; }
    }
}
