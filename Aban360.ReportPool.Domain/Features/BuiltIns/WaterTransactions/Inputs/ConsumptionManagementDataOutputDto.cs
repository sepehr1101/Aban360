namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ConsumptionManagementDataOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string BillId { get; set; }
        public string CustomerNumber { get; set; }

        public long BaseSumItems { get; set; }
        public float BaseContractOlgoo { get; set; }
        public float BaseConsumptionAverage { get; set; }

        public long ComparisonSumItems { get; set; }
        public float ComparisonContractOlgoo { get; set; }
        public float ComparisonConsmptionAverage { get; set; }

        public float PercentConsumptinAverageChange { get; set; }
        public float PercentSumItemsChange { get; set; }

        public string ReadingNumber { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string FullName { get; set; }
        public string UsageTitle { get; set; }
        public string BranchTypeTitle { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string SiphonDiameterTitle { get; set; }
    }
}
