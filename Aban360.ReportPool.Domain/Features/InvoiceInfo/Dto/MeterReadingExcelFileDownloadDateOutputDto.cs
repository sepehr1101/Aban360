namespace Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto
{
    public record MeterReadingExcelFileDownloadDateOutputDto
    {
        public string CurrentNumber { get; set; } = string.Empty;
        public string CurretnDateJalali { get; set; } = string.Empty;
        public string CurrentCounterStateCode { get; set; } = string.Empty;
        public string AgentCode { get; set; } = string.Empty;

        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string ReadingNumber { get; set; }
        public int PreviousNumber { get; set; }
        public string PreviousDateJalali { get; set; }
        public int PreviousCounterStateCode { get; set; }
        public string PreviousRegisterDateJalali { get; set; }
        public string FullName { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public int BranchTypeId { get; set; }
        public string BranchTypeTitle { get; set; }

    }
}
