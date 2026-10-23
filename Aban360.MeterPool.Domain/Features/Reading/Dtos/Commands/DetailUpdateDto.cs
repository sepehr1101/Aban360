namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Commands
{
    public record DetailUpdateDto
    {
        public Guid Id { get; set; }
    
        public int? CounterNumber { get; set; }
        public string? LocationDateTime { get; set; }
        public string? PhoneDateTime { get; set; }
        public int CurrentCounterStateCode { get; set; }
        
        public string? Y { get; set; }
        public string? X { get; set; }
        public string? GisAccuracy { get; set; }

        public bool? CounterNumberShown { get; set; }
        public string? Description { get; set; }
        public int? AttemptCount { get; set; }
        public bool? IsLocked { get; set; }
        public int? HighLowStateId { get; set; }

        public DateTime? OffLoadDateTime { get; set; }
        public string? OffloadDateJalali { get; set; }
        public string? OffLoadTime { get; set; }
        public Guid? OffLoadBy { get; set; }
    }
}
