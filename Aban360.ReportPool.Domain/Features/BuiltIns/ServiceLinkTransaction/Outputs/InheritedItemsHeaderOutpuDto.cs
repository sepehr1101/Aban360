namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record InheritedItemsHeaderOutpuDto
    {
        public int InheritedPrimises { get; set; }
        public int InheritedImprovementOverall { get; set; }
        public int InheritedImprovementCommericial { get; set; }
        public int InheritedImprovementDomestic { get; set; }
        public int InheritedImprovementOther { get; set; }
        public int InheritedUnitCommericial { get; set; }
        public int InheritedUnitDomestic { get; set; }
        public int InheritedUnitOther { get; set; }
        public int InheritedContractualCapacity { get; set; }
        public int SumInheritedPremisesImprovement { get; set; }
        public string? Title { get; set; }

    }
}
