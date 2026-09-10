namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record BillReadingListInputDto
    {
        public int ZoneId { get; set; }
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }
    }
}
