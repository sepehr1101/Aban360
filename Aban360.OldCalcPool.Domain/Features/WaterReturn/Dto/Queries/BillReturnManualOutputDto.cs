namespace Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries
{
    public record BillReturnManualOutputDto
    {
        public string BillId { get; set; }
        public string ReportDateJalali { get; set; }
        public string Title { get; set; }
        public long Amount { get; set; }
        public string ReturnCauseTitle { get; set; }
        public string FullName { get; set; }
        public string ZoneTitle { get; set; }
    }
}
