namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record TrackingDataDto
    {
        public Guid Id { get; set; }
        public Guid FirstId { get; set; }
        public Guid BlockId { get; set; }
        public long TrackNumber { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; } = null!;
        public int FlowStepId { get; set; }
        public string FlowStepTitle { get; set; }
        public string FromReadingNumber { get; set; } = null!;
        public string ToReadingNumber { get; set; } = null!;
        public Guid InsertBy { get; set; }
        public string InsertDateJalali { get; set; } = null!;
        public DateTime InsertDateTime { get; set; }
        public int CounterReaderCode { get; set; }
        public Guid CounterReaderId { get; set; }
        public string CounterReaderName { get; set; } = null!;
        public string CounterReaderMobile { get; set; } = null!;
        public DateTime PreviousInsertDate { get; set; }
        public int DistanceWithPreviousStep { get; set; }
    }
}