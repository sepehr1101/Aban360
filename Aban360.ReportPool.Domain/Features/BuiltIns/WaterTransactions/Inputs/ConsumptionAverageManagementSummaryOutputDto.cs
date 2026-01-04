namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ConsumptionAverageManagementSummaryOutputDto
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string NationalCode { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string MeterDiameterTitle { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public float Consumption { get; set; }
        public float ConsumptionAverage { get; set; }
        public int ContractualOrOlgo { get; set; }
        public string RegisterDateJalali { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
    }
}