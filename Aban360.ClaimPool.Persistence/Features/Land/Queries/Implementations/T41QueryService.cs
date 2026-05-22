using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    internal sealed class T41QueryService : AbstractBaseConnection, IT41QueryService
    {
        public T41QueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<NumericDictionary>> Get()
        {
            string query = GetQuery();
            IEnumerable<NumericDictionary> result = await _sqlReportConnection.QueryAsync<NumericDictionary>(query);
            return result;
        }

        public async Task<NumericDictionary> Get(int id, bool hasException)
        {
            string query = GetByIdQuery();
            NumericDictionary? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<NumericDictionary>(query, new { id });
            if (result is null && hasException)
            {
                throw new InvalidDataException(ExceptionLiterals.InvalidUsageTitle);
            }
            return result is null ? new NumericDictionary(id, string.Empty) : result;
        }

        private string GetQuery()
        {
            return @"Select 
                    	C0 Id,
                    	C1 Title
                    From [Db70].dbo.T41";
        }
        private string GetByIdQuery()
        {
            return @"Select 
                    	C0 Id,
                    	C1S Title
                    From [Db70].dbo.T41
                    Where C0=@id";
        }
    }
}
