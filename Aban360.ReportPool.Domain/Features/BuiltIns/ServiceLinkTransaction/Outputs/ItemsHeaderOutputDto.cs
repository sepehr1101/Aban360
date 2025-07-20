namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record ItemsHeaderOutputDto
    {
        public int Primises { get; set; }
        public int ImprovementOverall { get; set; }
        public int ImprovementCommericial { get; set; }
        public int ImprovementDomestic { get; set; }
        public int ImprovementOther { get; set; }
        public int UnitCommericial { get; set; }
        public int UnitDomestic { get; set; }
        public int UnitOther { get; set; }
        public int ContractualCapacity { get; set; }
        public int SumPremisesImprovement { get; set; }
        public int InheritedFromCustomerNumber { get; set; }
    }
}
