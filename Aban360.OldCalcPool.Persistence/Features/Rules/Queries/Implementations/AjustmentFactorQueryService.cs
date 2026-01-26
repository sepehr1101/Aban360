using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Implementations
{
    internal sealed class AjustmentFactorQueryService : AbstractBaseConnection, IAjustmentFactorQueryService
    {
        public AjustmentFactorQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IEnumerable<AjustmentFactorGetDto>> Get()
        {
            string query = GetAllQuery();
            IEnumerable<AjustmentFactorGetDto> result = await _sqlReportConnection.QueryAsync<AjustmentFactorGetDto>(query, null);
            return result;
        }
        public async Task<AjustmentFactorGetDto> Get(int zoneId)
        {
            string query = GetByZoneIdQuery();
            AjustmentFactorGetDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<AjustmentFactorGetDto>(query, new { zoneId });
            return result;
        }
        private string GetAllQuery()
        {
            return @"Select 
                    	Id,
                    	ZoneId,
                    	AjustmentFactor,
                    	Price
                    From OldCalc.dbo.AjustmentFactor";
        }
        private string GetByZoneIdQuery()
        {
            return @"Select 
                    	Id,
                    	ZoneId,
                    	AjustmentFactor,
                    	Price
                    From OldCalc.dbo.AjustmentFactor
                    Where ZoneId=@zoneId";
        }
    }
}