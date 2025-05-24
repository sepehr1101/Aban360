namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record EstatesInfoDto
    {
        //Estate Sepecification
        public int ImprovementsOverall { get; set; }
        public int ImprovementsDomestic { get; set; }
        public int ImprovementsCommercial { get; set; }
        public int ImprovementsOther { get; set; }
        public short Storeys { get; set; }
        public int ContractualCapacity { get; set; }
        public short UsageSellId { get; set; }//
        public short UsageConsumtion { get; set; }//
        public short ConstructionType { get; set; }
        public short flatCount { get; set; }



        //Flat Sepecification
        public short UnitDomesticWater { get; set; }
        public short UnitCommercialWater { get; set; }
        public short UnitOtherWater { get; set; }
        public short UnitDomesticSewage { get; set; }
        public short UnitCommercialSewage { get; set; }
        public short UnitOtherSewage { get; set; }
    }
}
