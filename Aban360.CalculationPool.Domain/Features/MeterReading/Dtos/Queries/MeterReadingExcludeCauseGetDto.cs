namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingExcludeCauseGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsSelectable { get; set; }
        public MeterReadingExcludeCauseGetDto(int id, string title, bool isSelectable)
        {
            Id = id;
            Title = title;
            IsSelectable = isSelectable;

        }
        public MeterReadingExcludeCauseGetDto()
        {
        }
    }
}
