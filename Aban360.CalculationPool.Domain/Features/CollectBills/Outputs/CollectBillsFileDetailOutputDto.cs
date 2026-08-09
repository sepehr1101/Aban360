namespace Aban360.CalculationPool.Domain.Features.CollectBills.Outputs
{
    public record CollectBillsFileDetailOutputDto
    {
        public string NameAndExtension { get; set; }
        public string CompressedFileName { get; set; }
        public string CompressedFileExtension { get; set; }
        public long? Year { get; set; }
        public long? CycleInYear { get; set; }
        public string Description { get; set; }
        public long? TraceNumber { get; set; }
        public string DateUploaded { get; set; }
        public string DateConfirmed { get; set; }
        public string DateArchived { get; set; }
        public string FileStatusName { get; set; }
        public string FileStatusShowName { get; set; }
        public string ArchiveDescriptionByUser { get; set; }
        public string ArchiveDescriptionBySystem { get; set; }
        public string CorrectBillAmount { get; set; }
        public string WarningBillAmount { get; set; }
        public long? RecordCount { get; set; }
        public long? BillCount { get; set; }
        public long? WarningBillCount { get; set; }
        public long? CorrectBillCount { get; set; }
        public long? ErrorBillCount { get; set; }
        public string ErrorBillAmount { get; set; }
        public long FileID { get; set; }
    }
}