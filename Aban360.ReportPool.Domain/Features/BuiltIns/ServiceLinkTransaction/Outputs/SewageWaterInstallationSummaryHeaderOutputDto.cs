namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record SewageWaterInstallationSummaryHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        
        public string? FromReadingNumber{ get; set; }
        public string? ToReadingNumber { get; set; }

        public string Title { get; set; }
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }

        public int SumDomesticUnit { get; set; }
        public int SumCommercialUnit { get; set; }
        public int SumOtherUnit { get; set; }
        public int TotalUnit { get; set; }
        public long ContractualCapacity { get; set; }

        public int UnSpecified { get; set; }
        public int Field0_5 { get; set; }
        public int Field0_75 { get; set; }
        public int Field1 { get; set; }
        public int Field1_2 { get; set; }
        public int Field1_5 { get; set; }
        public int Field2 { get; set; }
        public int Field3 { get; set; }
        public int Field4 { get; set; }
        public int Field5 { get; set; }
        public int MoreThan6 { get; set; }
    }
}
