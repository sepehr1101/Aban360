using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class FlatInfoService : AbstractBaseConnection, IFlatInfoService
    {
        public FlatInfoService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<FlatInfoDto> GetInfo(string billId)
        {
            string branchHistoryQuery = GetFlatSummayDtoQuery();
            FlatInfoDto result= await _sqlConnection.QueryFirstOrDefaultAsync<FlatInfoDto>(branchHistoryQuery, new { billId });

            return result;
        }
        private string GetFlatSummayDtoQuery()
        {
            return @"select 
                    	f.Id as 'FlatId',
                    	u.Title as 'UsageTitle',
                    	N'---' as 'SecondaryWaterUse',
                    	u.Title as 'BusinessCategory',
                    	12 as 'Capacity',
                    	cc.Title as 'CapacityCalculationIndexTitle',
                    	N'---' as 'CapacityCalculationIndexValue',
                    	e.ImprovementsCommercial as 'ImprovementUnitArea',
                    	0 as 'VacancyStatus'
                    from ClaimPool.WaterMeter w 
                    join ClaimPool.Estate e on w.EstateId=e.Id
                    join ClaimPool.Flat f on f.EstateId=e.Id
                    join ClaimPool.Usage u on e.UsageConsumtionId=u.Id
                    join ClaimPool.CapacityCalculationIndex cc on e.CapacityCalculationIndexId=cc.Id
                      where w.BillId=@billId";

        }
    }
}
