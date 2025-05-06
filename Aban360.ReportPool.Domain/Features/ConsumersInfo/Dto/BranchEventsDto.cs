namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record BranchEventsDto
    {
        public string Description { get; set; } = default!;
        public string TrackNumber { get; set; } = default!;
        public string RegisterDate { get; set; } = default!;
        public long DebtAmount { get; set; }
        public long CreditAmount { get; set; }
    }
}