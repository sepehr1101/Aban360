using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record ConnectDisconnectPrintDataOutputDto
    {
        public int CustomerNumber { get; set; }
        public string RegionTitle { get; set; }
        public string ZoneTitle { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public long WaterDebtAmount { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public string FullName { get; set; }
        public string? NationalCode { get; set; }
        public string MobileNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string BillId { get; set; }
        public string PaymentId { get; set; }
        public string UsageTitle { get; set; }
        public int MeterDiameterId { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string BranchTypeTitle { get; set; }
        public string CauseTitle { get; set; }
        public string CompanyTitle{ get; set; }
        public string? Base64 { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string X { get; set; }
        public string Y { get; set; }
    }
}
