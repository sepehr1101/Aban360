using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    internal sealed class ModifyCauseQueryService : AbstractBaseConnection, IModifyCauseQueryService
    {
        public ModifyCauseQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<NumericDictionary>> Get()
        {
            string query = GetQuery();
            return await _sqlReportConnection.QueryAsync<NumericDictionary>(query);
        }

        public async Task<NumericDictionary> Get(int id, bool hasException = true)
        {
            string query = GetByIdQuery();
            NumericDictionary? data = await _sqlReportConnection.QueryFirstOrDefaultAsync<NumericDictionary>(query, new { id });
            if (data is null && hasException)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.InvalidModifyCauseId);
            }

            return data;
        }

        private string GetQuery()
        {
            return @"Select 
                    	Id,
                        Title
                    From [Db70].dbo.ModifyCause";
        }
        private string GetByIdQuery()
        {
            return @$"Select 
                    	Id,
                        Title
                    From [Db70].dbo.ModifyCause
                    Where Id = @id";
        }
    }
}
