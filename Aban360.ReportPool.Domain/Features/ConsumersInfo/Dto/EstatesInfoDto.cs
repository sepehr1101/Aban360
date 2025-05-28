namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record EstatesInfoDto
    {
        //Estate Sepecification
        public int  Premises { get; set; }
        public int UnitOverall { get; set; }
        public int ImprovementsOverall { get; set; }
        public int ImprovementsDomestic { get; set; }

        public int ImprovementsCommercial { get; set; }
        public int ImprovementsOther { get; set; }
        public string OwnershipTypeTitle { get; set; }//

        public string UsageSellTitle { get; set; }
        public string DebtCollectionGroupTitle { get; set; }//
        public short flatCount { get; set; }

        public string ContractualCapacity { get; set; }
        public string ConstructionTypeTitle { get; set; }
        public short Storeys { get; set; }




        //Flat Sepecification
        public short UnitDomesticWater { get; set; }
        public short UnitCommercialWater { get; set; }
        public short UnitOtherWater { get; set; }

        public short UnitDomesticSewage { get; set; }
        public short UnitCommercialSewage { get; set; }
        public short UnitOtherSewage { get; set; }
    }
}
