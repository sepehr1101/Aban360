namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record GenerateBillInputDto
    {
        public string BillId { get; set; }
        public int MeterNumber { get; set; }
        public int? CounterStateCode { get; set; }
        public string? ClientDateTime { get; set; }
        public double? X { get; set; }
        public double? Y { get; set; }
        public double? Precision { get; set; }

        public bool IsConfirm { get; set; }

    }
}
