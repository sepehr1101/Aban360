using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class T9QueryService : AbstractBaseConnection, IT9QueryService
    {
        public T9QueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<NumericDictionary>> Get()
        {
            string query = GetQuery();
            IEnumerable<NumericDictionary> result = await _sqlReportConnection.QueryAsync<NumericDictionary>(query, null);
            return result;
        }
        public async Task<NumericDictionary> Get(int Id)
        {
            string query = GetByIdQuery();
            NumericDictionary result = await _sqlReportConnection.QueryFirstOrDefaultAsync<NumericDictionary>(query, new { Id });
            return result;

        }
        public async Task<IEnumerable<NumericDictionary>> GetByTypeId(int typeId)
        {
            string query = GetByTypeIdQuery();
            IEnumerable<NumericDictionary> result = await _sqlReportConnection.QueryAsync<NumericDictionary>(query, new { typeId });
            return result;
        }

        private string GetQuery()
        {
            return @"Select 
                    	C0 Id,
                    	C3 Title
                    From Db70.dbo.T9";
        }
        private string GetByIdQuery()
        {
            return @"Select 
                    	C0 Id,
                    	C3 Title
                 From Db70.dbo.T9
                 Where C0=@id";
        }
        private string GetByTypeIdQuery()
        {
            return @"Select 
                    	C0 Id,
                    	C3 Title
                 From [Db70].dbo.T9
                 Where C1=@typeId";
        }
    }
}
