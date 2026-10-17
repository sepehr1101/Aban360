namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record BoundInsertDto
    {
        public Guid Id { get; set; } 
        public int ReadingBoundId { get; set; }
        public string ReadingBoundTitle { get; set; } = null!;
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; } = null!;
        public string FromReadingNumber { get; set; } = null!;
        public string ToReadingNumber { get; set; } = null!;
        public bool IsValidated { get; set; }
        public int BlockCount { get; set; }
        public int CustomerCount { get; set; }
        public int Year { get; set; }
        public int ReadingPeriodTypeId { get; set; }
        public int ReadingPeriodId { get; set; }
        public Guid InsertBy { get; set; }
        public DateTime InsertDateTime { get; set; }
    }
}
