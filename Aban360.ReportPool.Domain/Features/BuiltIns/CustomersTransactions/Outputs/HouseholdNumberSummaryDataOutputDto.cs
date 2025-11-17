namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record HouseholdNumberSummaryDataOutputDto
    {
        public bool IsFirstRow { get; set; }

        public string ItemTitle { get; set; }
        public float SumHousehold { get; set; }
        public int CustomerCount { get; set; }
        public int ValidCount { get; set; }
        public int InvalidCount { get; set; }
        public int TotalUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }
        public int Field1 { get; set; }
        public int Field2 { get; set; }
        public int Field3 { get; set; }
        public int Field4 { get; set; }
        public int FieldMore5 { get; set; }

    }
}
