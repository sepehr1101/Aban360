namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record PreviousItemsHeaderOutpuDto
    {
        public int PreviousPrimises { get; set; }
        public int PreviousImprovementOverall { get; set; }
        public int PreviousImprovementCommericial { get; set; }
        public int PreviousImprovementDomestic { get; set; }
        public int PreviousImprovementOther { get; set; }
        public int PreviousUnitCommericial { get; set; }
        public int PreviousUnitDomestic { get; set; }
        public int PreviousUnitOther { get; set; }
        public int PreviousContractualCapacity { get; set; }
        public int SumPreviousPremisesImprovement { get; set; }
        public int InheritedFromCustomerNumber { get; set; }


    }
}
