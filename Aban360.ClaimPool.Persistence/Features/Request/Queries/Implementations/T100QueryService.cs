using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Exceptions;
using Aban360.Common.Exceptions;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class T100QueryService : AbstractBaseConnection, IT100QueryService
    {
        public T100QueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<NumericDictionary> Get(int Id, bool hasException)
        {
            string query = GetByIdQuery();
            NumericDictionary? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<NumericDictionary>(query, new { Id });
            if (hasException && result is null)
            {
                throw new InvalidTrackingException(ExceptionLiterals.NotFoundT100);
            }
            return result;
        }
        public async Task<IEnumerable<NumericDictionary>> Get()
        {
            string query = GetQuery();
            IEnumerable<NumericDictionary> result = await _sqlReportConnection.QueryAsync<NumericDictionary>(query, null);
            return result;
        }
        private string GetByIdQuery()
        {
            return @"Select 
                    	C0 Id,
                    	C1 Title
                    From Db70.dbo.T100
                    Where C0=@id";
        }
        private string GetQuery()
        {
            return @"Select 
                    	C0 Id,
                    	C1 Title
                    From Db70.dbo.T100";
        }
    }
}
