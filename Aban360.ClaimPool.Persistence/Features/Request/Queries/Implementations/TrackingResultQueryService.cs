using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class TrackingResultQueryService : AbstractBaseConnection, ITrackingResultQueryService
    {
        public TrackingResultQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<NumericDictionary>> Get()
        {
            string query = GetAllQuery();
            return await _sqlReportConnection.QueryAsync<NumericDictionary>(query, null);
        }
        private string GetAllQuery()
        {
            return @"Select 
                    	C0 Id,
                    	C1 Title
                    From Db70.dbo.t64";
        }
    }
}
