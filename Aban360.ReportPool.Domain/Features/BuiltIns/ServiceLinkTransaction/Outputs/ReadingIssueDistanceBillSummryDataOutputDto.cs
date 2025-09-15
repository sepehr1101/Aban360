namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record ReadingIssueDistanceBillSummryDataOutputDto
    {
        public string ItemTitle { get; set; }
        public int CustomerCount { get; set; }
        public float TotalUnit { get; set; }
        public float CommercialUnit { get; set; }
        public float DomesticUnit { get; set; }
        public float OtherUnit { get; set; }

        public float UnSpecified { get; set; }
        public string UnSpecifiedText { get; set; }

        public float Field0_5 { get; set; }
        public string Field0_5Text { get; set; }

        public float Field0_75 { get; set; }
        public string Field0_75Text { get; set; }

        public float Field1 { get; set; }
        public string Field1Text { get; set; }

        public float Field1_2 { get; set; }
        public string Field1_2Text { get; set; }

        public float Field1_5 { get; set; }
        public string Field1_5Text { get; set; }

        public float Field2 { get; set; }
        public string Field2Text { get; set; }

        public float Field3 { get; set; }
        public string Field3Text { get; set; }

        public float Field4 { get; set; }
        public string Field4Text { get; set; }

        public float Field5 { get; set; }
        public string Field5Text { get; set; }

        public float MoreThan6 { get; set; }
        public string MoreThan6Text { get; set; }
    }
}
         