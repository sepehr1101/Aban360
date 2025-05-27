using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class SpecialConditionTagsInfoService : AbstractBaseConnection, ISpecialConditionTagsInfoService
    {
        public SpecialConditionTagsInfoService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<SpecialConditionTagsInfoDto> GetInfo(string billId)
        {
            string branchHistoryQuery = GetSpecialConditionTagsSummayDtoQuery();
            string waterMeterTagsQuery = GetWaterMeterTagsDtoQuery();

            SpecialConditionTagsInfoDto result = await _sqlConnection.QueryFirstOrDefaultAsync<SpecialConditionTagsInfoDto>(branchHistoryQuery, new { billId });
            result.WaterMeterTags = await _sqlConnection.QueryAsync<WaterMeterTagInfoDto>(waterMeterTagsQuery, new { billId });
            
            return result;
        }
        private string GetSpecialConditionTagsSummayDtoQuery()
        {
            return @"select
                    	N'عادی' as 'HandoverTitle',--without relation
                    	N'---' as 'CoverageStatus',
                    	us.Title as 'UsageStateTitle',
                    	0 as 'SpecialBranch',
                    	2 as 'VacantUnitCount',
                    	e.HouseholdNumber as 'HouseholderNumber',
                    	1 as 'NonSequentialFlag'
                    from ClaimPool.WaterMeter w 
                    join ClaimPool.WaterMeterTag wt on w.Id=wt.WaterMeterId
                    join ClaimPool.WaterMeterTagDefinition wtd on wtd.Id=wt.WaterMeterTagDefinitionId
                    join ClaimPool.UseState us on w.UseStateId=us.Id
                    join ClaimPool.Estate e on w.EstateId=e.Id
                         where w.BillId=@billId";

        }
        private string GetWaterMeterTagsDtoQuery()
        {
            return @"select
                    	wtd.Id,
                    	wtd.Title
                    from ClaimPool.WaterMeter w 
                    join ClaimPool.WaterMeterTag wt on w.Id=wt.WaterMeterId
                    join ClaimPool.WaterMeterTagDefinition wtd on wtd.Id=wt.WaterMeterTagDefinitionId
                         where w.BillId=@billId";
        }
    }
}
