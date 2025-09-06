namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs
{
    public record RemovedBillDetailDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public int ZoneId { get; set; }
        public string BillId { get; set; }
        public int CurrentMeterNumber { get; set; }
        public int PreviousMeterNumber { get; set; }
        public string CurrentDayJalali { get; set; }
        public string PreviousDayJalali { get; set; }
        public int Consumption { get; set; }
        public long Amount { get; set; }
        public string RemovedDateJalali { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string NationalCode { get; set; }
        public string PostalCode { get; set; }
        public string UsageTitle { get; set; }
    }
}
