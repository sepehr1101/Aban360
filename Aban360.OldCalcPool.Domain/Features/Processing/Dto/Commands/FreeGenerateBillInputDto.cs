namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands
{
    public record FreeGenerateBillInputDto
    {
        public string BillId { get; set; }
        public int CurrentMeterNumber { get; set; }
        public int PreviousMeterNumber { get; set; }
        public string CurrentDateJalali { get; set; }
        public string PreviousDateJalali { get; set; }
        public float? ConsumptionAverage { get; set; }
        public int? CounterStateCode { get; set; }

        public bool IsConfirm { get; set; }

    }
}


