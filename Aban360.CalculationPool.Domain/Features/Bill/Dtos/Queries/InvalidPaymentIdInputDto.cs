namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record InvalidPaymentIdInputDto
    {
        public int ZoneId { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
    }
}
