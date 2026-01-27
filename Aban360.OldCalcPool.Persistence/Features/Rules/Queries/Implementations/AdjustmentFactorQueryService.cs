using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Implementations
{
    internal sealed class AdjustmentFactorQueryService : AbstractBaseConnection, IAdjustmentFactorQueryService
    {
        public AdjustmentFactorQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IEnumerable<AdjustmentFactorGetDto>> Get()
        {
            string query = GetAllQuery();
            IEnumerable<AdjustmentFactorGetDto> result = await _sqlReportConnection.QueryAsync<AdjustmentFactorGetDto>(query, null);
            return result;
        }
        public async Task<AdjustmentFactorGetDto> Get(int zoneId)
        {
            string query = GetByZoneIdQuery();
            AdjustmentFactorGetDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<AdjustmentFactorGetDto>(query, new { zoneId });
            return result;
        }
        private string GetAllQuery()
        {
            return @"Select 
                    	Id,
                    	ZoneId,
                    	AdjustmentFactor,
                    	Price
                    From OldCalc.dbo.AdjustmentFactor";
        }
        private string GetByZoneIdQuery()
        {
            return @"Select 
                    	Id,
                    	ZoneId,
                    	AdjustmentFactor,
                    	Price
                    From OldCalc.dbo.AdjustmentFactor
                    Where ZoneId=@zoneId";
        }
    }
}