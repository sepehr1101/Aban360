namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record ChangeDateBatchOutputDto
    {
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }
        public string DateJalali { get; set; }
        public int Count { get; set; }
        public ChangeDateBatchOutputDto(string fromReadingNumber, string toReadingNumber, string dateJalali, int count)
        {
            FromReadingNumber = fromReadingNumber;
            ToReadingNumber = toReadingNumber;
            DateJalali = dateJalali;
            Count = count;
        }
        public ChangeDateBatchOutputDto()
        {
        }
    }
}
