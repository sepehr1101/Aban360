namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record SewageWaterInstallationSummaryByZoneIdKeyDataOutputDto
    {
        public string RegionTitle { get; set; }
        public int SumCustomerCount { get; set; }
        public int SumTotalUnit { get; set; }
        public int SumUnSpecified { get; set; }
        public int SumField0_5 { get; set; }
        public int SumField0_75 { get; set; }
        public int SumField1 { get; set; }
        public int SumField1_2 { get; set; }
        public int SumField1_5 { get; set; }
        public int SumField2 { get; set; }
        public int SumField3 { get; set; }
        public int SumField4 { get; set; }
        public int SumField5 { get; set; }
        public int SumMoreThan6 { get; set; }
    }
    
    public record SewageWaterInstallationSummaryByZoneIdDataOutputDto
    {
        public string RegionTitle { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerCount { get; set; }
        public int TotalUnit { get; set; }
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

    public record SewageWaterInstallationSummaryByZoneIdDataGroupOutputDto
    {
        public SewageWaterInstallationSummaryByZoneIdKeyDataOutputDto Key { get; set; }
        public IEnumerable<SewageWaterInstallationSummaryByZoneIdValueDataOutputDto>  Values  { get; set; }
    }
}
