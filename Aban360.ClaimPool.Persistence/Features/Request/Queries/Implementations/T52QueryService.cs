using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class T52QueryService : AbstractBaseConnection, IT52QueryService
    {
        public T52QueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<string> Get(ZoneIdAndCustomerNumber input)
        {
            string query = GetByZoneIdAndCustomerNumberQuery();
            string? _3digitZoneId = await _sqlConnection.QueryFirstOrDefaultAsync<string>(query, input);
            if (string.IsNullOrWhiteSpace(_3digitZoneId))
            {
                throw new InvalidBillCommandException(ExceptionLiterals.NotFoundUniqueCode);
            }
            return _3digitZoneId;
        }

        private string GetByZoneIdAndCustomerNumberQuery()
        {
            return @"Select C3 _3digitZoneId
                    From Db70.dbo.T52
                    Where 
                        C1=@ZoneId AND 
                        (@CustomerNumber Between C9 and C10)";
        }
    }
}
