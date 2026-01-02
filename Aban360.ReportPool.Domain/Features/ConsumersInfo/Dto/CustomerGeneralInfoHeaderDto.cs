namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record CustomerGeneralInfoHeaderDto
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string NationalCode { get; set; }
        public string MobileNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string BillId { get; set; }
        public string UsageTitle { get; set; }
        public int ContractualCapacity { get; set; }
        public string MeterDiameterId { get; set; }
        public string MainSiphon { get; set; }
        public string BranchTypeTitle { get; set; }
        public bool DiscountType { get; set; }
        public string WaterRequestDateJalali { get; set; }
        public string WaterInstallationDateJalali { get; set; }
        public string SewageRequestDateJalali { get; set; }
        public string SewageInstallationDateJalali { get; set; }

        public string ReportDateJalali { get; set; }
        public string Title { get; set; }

    }
}
