namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record ReadingCounterStateGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool CanEnterNumber { get; set; }
        public bool IsClose { get; set; }
        public bool CanPriGTCurrent { get; set; }
        public bool IsChange { get; set; }
        public bool ShouldNumber { get; set; }
        public bool IsMalfunction { get; set; }
        public bool HasMeter { get; set; }
        public bool HasImage { get; set; }
    }
}
