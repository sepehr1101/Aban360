namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record EventsSummaryDto
    {
        public string BillId { get; set; } = default!;
        public long Id { get; set; }
        public int? PreviousMeterNumber { get; set; }//
        public int? NextMeterNumber { get; set; }//
        public string? Description { get; set; }//
        public string Style { get; set; } = default!;
        public long? DebtAmount { get; set; }//
        public long? OweAmount { get; set; }//
        public string? PreviousMeterDate { get; set; }//
        public string? CurrentMeterDate { get; set; }//
        public string RegisterDate { get; set; } = default!;//
        public float? ConsumptionAverage { get; set; }//
        public string? BankTitle { get; set; }//
    }
}
