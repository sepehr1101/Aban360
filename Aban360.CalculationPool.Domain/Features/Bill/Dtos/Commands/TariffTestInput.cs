namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record TariffTestInput
    {
        public int PreviousReadingNumber { get; set; }
        public int CurrentReadingNumber { get; set; }
        public string PreviousReadingDate { get; set; } = default!;
        public string BillId { get; set; } = default!;
    }
}
