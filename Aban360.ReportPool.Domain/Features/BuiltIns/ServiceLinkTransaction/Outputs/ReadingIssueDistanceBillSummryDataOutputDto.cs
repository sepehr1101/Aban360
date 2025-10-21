namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record ReadingIssueDistanceBillSummryDataOutputDto
    {
        public bool  IsFirstRow { get; set; }
        
        public string RegionTitle { get; set; }
        public string ZoneTitle { get; set; }
        public string UsageTitle { get; set; }
        public string ItemTitle { get; set; }
        public int BillCount { get; set; }
        public int TotalUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int DomesticUnit { get; set; }
        public int OtherUnit { get; set; }
        public float AverageAll { get; set; }
        public string AverageAllText { get; set; }

        public int UnSpecified { get; set; }
        public string UnSpecifiedText { get; set; }

        public int Field0_5 { get; set; }
        public string Field0_5Text { get; set; }

        public int Field0_75 { get; set; }
        public string Field0_75Text { get; set; }

        public int Field1 { get; set; }
        public string Field1Text { get; set; }

        public int Field1_2 { get; set; }
        public string Field1_2Text { get; set; }

        public int Field1_5 { get; set; }
        public string Field1_5Text { get; set; }

        public int Field2 { get; set; }
        public string Field2Text { get; set; }

        public int Field3 { get; set; }
        public string Field3Text { get; set; }

        public int Field4 { get; set; }
        public string Field4Text { get; set; }

        public int Field5 { get; set; }
        public string Field5Text { get; set; }

        public int MoreThan6 { get; set; }
        public string MoreThan6Text { get; set; }
    }
}
         