using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class MoshtrakQueryService : AbstractBaseConnection, IMoshtrakQueryService
    {
        public MoshtrakQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task CheckOpenRequest(string nationalCode, int zoneId)
        {
            string dbName = GetDbName(zoneId);
            string query = GetHasOpenRequestByNationalCodeQuery(dbName);
            string? trackNumber = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(query, new { nationalCode });
            if (!string.IsNullOrWhiteSpace(trackNumber))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidOpenRequest(trackNumber));
            }
        }
        public async Task CheckOpenRequest(int customerNumber, int zoneId)
        {
            string dbName = GetDbName(zoneId);
            string query = GetHasOpenRequestByCustomerNumberCodeQuery(dbName);
            string? trackNumber = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(query, new { customerNumber, zoneId });
            if (!string.IsNullOrWhiteSpace(trackNumber))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidOpenRequest(trackNumber));
            }
        }

        private string GetHasOpenRequestByNationalCodeQuery(string dbName)
        {
            return $@"Select TrackingNumber
                    From [{dbName}].dbo.Moshtrak 
                    Where 
                    	TRIM(meli_cod)=@NationalCode AND
                    	sabt=0";
        }
        private string GetHasOpenRequestByCustomerNumberCodeQuery(string dbName)
        {
            return $@"Select TrackingNumber
                    From [{dbName}].dbo.Moshtrak 
                    Where 
                    	radif=@customerNumber AND
						town=@zoneId AND
                    	sabt=0";
        }

    }
}
