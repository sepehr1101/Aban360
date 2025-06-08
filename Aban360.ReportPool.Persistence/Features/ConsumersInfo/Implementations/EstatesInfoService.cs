using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class EstatesInfoService : AbstractBaseConnection, IEstatesInfoService
    {
        public EstatesInfoService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<IEnumerable<EstatesInfoDto>> GetInfo(string billId)
        {
            //string individualsQuery = GetIndividualsSummayDtoQuery();
            string individualsQuery = GetIndividualsSummayDtoWithClientDbQuery();
            IEnumerable<EstatesInfoDto> result = await _sqlConnection.QueryAsync<EstatesInfoDto>(individualsQuery, new { billId });

            return result;
        }
        private string GetIndividualsSummayDtoQuery()
        {
            return @"select
			    		e.Premises,
                        e.UnitCommercialWater+e.UnitDomesticWater+e.UnitOtherWater as 'UnitOverall',
                    	e.ImprovementsCommercial,
                    	e.ImprovementsDomestic,
                    	e.ImprovementsOverall,
                    	e.ImprovementsOther,
			    		N'سند تک برگ' as 'OwnershipTypeTitle',
                    	u.Title as N'UsageSellTitle',
			    		N'---' as 'DebtCollectionGroupTitle',
			    		count(f.Storey) as flatCount,
			    		e.ContractualCapacity,
			    		ct.Title as N'ConstructionTypeTitle',
			    		e.Storeys,
			    		
			    		e.UnitCommercialWater,
                    	e.UnitDomesticWater,
                    	e.UnitOtherWater,
                    	e.UnitCommercialSewage,
                    	e.UnitDomesticSewage,
                    	e.UnitOtherSewage
                    from ClaimPool.WaterMeter w
                    join ClaimPool.Estate e on w.EstateId=e.Id
			    	join ClaimPool.ConstructionType ct on e.ConstructionTypeId=ct.Id
                    join ClaimPool.Usage u on e.UsageSellId=u.Id
                    join ClaimPool.Flat f on f.EstateId=e.Id
                    where w.BillId=@billId
                    group by 
			    		 e.Premises,
                    	e.ImprovementsCommercial,
                    	e.ImprovementsDomestic,
                    	e.ImprovementsOverall,
                    	e.ImprovementsOther,
                    	u.Title ,
			    		e.ContractualCapacity,
			    		ct.Title ,
			    		e.Storeys,
			    		
			    		e.UnitCommercialWater,
                    	e.UnitDomesticWater,
                    	e.UnitOtherWater,
                    	e.UnitCommercialSewage,
                    	e.UnitDomesticSewage,
                	    e.UnitOtherSewage";
                
        }

        private string GetIndividualsSummayDtoWithClientDbQuery()
        {
            return @"select 
                        c.FieldArea As Premises,
                    	c.CommercialCount+c.DomesticCount+c.OtherCount AS UnitOverall,
                    	c.ConstructedArea AS ImprovementsOverall,
                    	c.DomesticArea AS ImprovementsDomestic,
                    	c.CommercialArea AS ImprovementsCommercial,
                    	c.ConstructedArea-DomesticArea-CommercialArea AS ImprovementsOther,
                    	N'نامشخص' AS OwnershipTypeTitle,
                    	c.UsageTitle2 AS UsageSellTitle,
                    	'' AS DebtCollectionGroupTitle,
                    	0 AS flatCount,
                    	c.ContractCapacity AS ContractualCapacity,
                    	N'نامشخص' AS ConstructionTypeTitle,
                    	0 AS Storeys,
                    	c.DomesticCount AS UnitDomesticWater,
                    	c.DomesticCount AS UnitDomesticSewage,
                    	c.CommercialCount AS UnitCommercialWater,
                    	c.CommercialCount AS UnitCommercialSewage,
                    	c.OtherCount AS UnitOtherWater,
                    	c.OtherCount AS UnitOtherSewage                    
                    from Client1000 c
                    where c.BillId=@billId
                    and c.ToDayJalali is null
        ";
        }
    }
}
