using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    internal sealed class T46QueryService : AbstractBaseConnection, IT46QueryService
    {
        public T46QueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<NumericDictionary>> Get()
        {
            string query = GetQuery();
            IEnumerable<NumericDictionary> result = await _sqlReportConnection.QueryAsync<NumericDictionary>(query);
            return result;
        }
        public async Task<NumericDictionary> GetByZone(int zoneId, bool hasException)
        {
            string query = GetByZoneIdQuery();
            NumericDictionary? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<NumericDictionary>(query, new { zoneId });
            if (result is null && hasException)
            {
                throw new InvalidCustomerCommandException(ExceptionLiterals.InvalidZoneId);
            }
            return result;
        }
        private string GetQuery()
        {
            return @"Select 
                    	C0 Id,
                    	C2 Title
                    From [Db70].dbo.T46";
        }
        private string GetByZoneIdQuery()
        {
            return @"Select 
                    	C0 Id,
                    	C2 Title
                    From [Db70].dbo.T46";
        }
    }
}
