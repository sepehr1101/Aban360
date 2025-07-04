﻿using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
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

        public async Task<IEnumerable<FlatInfoDto>> GetInfo(string billId)
        {
            string branchHistoryQuery = GetFlatSummayDtoQuery();
           // FlatInfoDto result= await _sqlConnection.QueryFirstOrDefaultAsync<FlatInfoDto>(branchHistoryQuery, new { billId });

            FlatInfoDto[] result = new[] { new FlatInfoDto() };
            return result;
        }
        private string GetFlatSummayDtoQuery()
        {
            return @"select 
                    	ROW_NUMBER() OVER (ORDER BY f.Id) as 'FlatNumber',
                    	'---' as 'UsageTitle',
                    	u.Title as 'UsageConsumptionTitle',
                    	g.Title as 'GuildTitle',
                    	e.ContractualCapacity as 'ContractualCapacity',
                    	cc.Title as 'CapacityCalculationIndexTitle',
                    	N'---' as 'CapacityCalculationIndexValue',
                    	e.ImprovementsOverall as 'ImprovementsOverall',
                    	e.EmptyUnit as 'EmptyUnit'
                    from ClaimPool.WaterMeter w 
                    join ClaimPool.Estate e on w.EstateId=e.Id
                    join ClaimPool.Flat f on f.EstateId=e.Id
                    join ClaimPool.Usage u on e.UsageConsumtionId=u.Id
					join ClaimPool.Guild g on g.UsageId=u.Id
                    join ClaimPool.CapacityCalculationIndex cc on e.CapacityCalculationIndexId=cc.Id
                      where w.BillId=@billId";

        }
        
    }
}
