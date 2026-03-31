using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
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

        public async Task<NumericDictionary> Get(int Id)
        {
            string query = GetByIdQuery();
            NumericDictionary result = await _sqlConnection.QueryFirstOrDefaultAsync<NumericDictionary>(query, new { Id });
            return result;
        }
        public async Task<IEnumerable<NumericDictionary>> Get()
        {
            string query = GetQuery();
            IEnumerable<NumericDictionary> result = await _sqlConnection.QueryAsync<NumericDictionary>(query, null);
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
