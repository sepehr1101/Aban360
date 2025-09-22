namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record ReadingDailyStatementDataOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string ReadingNumber { get; set; }
        public string CustomerNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string MeterDiameterTitle { get; set; }
        public int Consumption { get; set; }
        public int ConsumptionAverage { get; set; }
        public long InvoiceAmount { get; set; }
        public string Address { get; set; }
        public string BillId { get; set; }
        public string UsageTitle { get; set; }
        public string PaymentId { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalCode { get; set; }
        public string PostalCode { get; set; }
        public string FromReadingDateJalali { get; set; }
        public string ToReadingDateJalali { get; set; }
        public string RegisterBillDateJalali { get; set; }
        public long SumItems { get; set; }
    }
}