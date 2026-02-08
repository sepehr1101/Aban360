using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class CompanyServiceQueryService : AbstractBaseConnection, ICompanyServiceQueryService
    {
        public CompanyServiceQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<NumericDictionary>> GetByTypeId(int typeId)
        {
            string query = GetByTypeIdQuery();
            IEnumerable<NumericDictionary> result = await _sqlConnection.QueryAsync<NumericDictionary>(query, new { typeId });
            return result;
        }
        private string GetByTypeIdQuery()
        {
            return @"Select 
                    	Id,
                    	Title
                    From Aban360.CalculationPool.CompanyService
                    Where CompanyServiceTypeId=@typeId";
        }
    }
}
