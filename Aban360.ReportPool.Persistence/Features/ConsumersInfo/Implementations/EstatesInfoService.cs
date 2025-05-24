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
            string individualsQuery = GetIndividualsSummayDtoQuery();
            IEnumerable<EstatesInfoDto> result = await _sqlConnection.QueryAsync<EstatesInfoDto>(individualsQuery, new { billId });

            return result;
        }
        private string GetIndividualsSummayDtoQuery()
        {
            return @"select
                	e.ImprovementsCommercial,
                	e.ImprovementsDomestic,
                	e.ImprovementsOverall,
                	e.ImprovementsOther,
                	e.UnitCommercialWater,
                	e.UnitDomesticWater,
                	e.UnitOtherWater,
                	e.UnitCommercialSewage,
                	e.UnitDomesticSewage,
                	e.UnitOtherSewage,
                	u1.Title as N'UsageConsumption',
                	u2.Title as N'UsageSell',
                	count(f.Storey) as flatCount
                from ClaimPool.WaterMeter w
                join ClaimPool.Estate e on w.EstateId=e.Id
                join ClaimPool.Usage u1 on e.UsageConsumtionId=u1.Id 
                join ClaimPool.Usage u2 on e.UsageSellId=u2.Id
                join ClaimPool.Flat f on f.EstateId=e.Id
                where w.BillId=@billId
                group by 
                     e.ImprovementsCommercial,
                     e.ImprovementsDomestic,
                     e.ImprovementsOverall,
                     e.ImprovementsOther,
                     e.UnitCommercialWater,
                     e.UnitDomesticWater,
                     e.UnitOtherWater,
                     e.UnitCommercialSewage,
                     e.UnitDomesticSewage,
                     e.UnitOtherSewage,
                     u1.Title,
                     u2.Title";

        }
    }
}
