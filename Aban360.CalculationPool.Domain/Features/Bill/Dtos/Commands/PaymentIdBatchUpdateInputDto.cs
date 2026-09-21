namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record PaymentIdBatchUpdateInputDto
    {
        public int ZoneId { get; set; }
        public ICollection<int> Ids { get; set; }
    }
}
