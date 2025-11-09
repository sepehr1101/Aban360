namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input
{
    public record MeterInfoByPreviousDataWithInvoiceIdInputDto
    {
        public string? BillId { get; set; }
        public string PreviousDateJalali { get; set; } = default!;
        public int PreviousNumber { get; set; }

        public string CurrentDateJalali { get; set; } = default!;
        public int CurrentMeterNumber { get; set; }

        public int InvoiceId { get; set; }
        public bool ShouldSave { get; set; }
        public decimal MeetingNumber { get; set; }
        public int Cause { get; set; }

    }
}
