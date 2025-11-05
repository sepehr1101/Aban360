using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Implementations
{
    internal sealed class SiphonDiameterQueryService : AbstractBaseConnection, ISiphonDiameterQueryService
    {
        public SiphonDiameterQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IEnumerable<NumericDictionary>> Get()
        {
            string query = GetAllQuery();
            IEnumerable<NumericDictionary> article11 = await _sqlConnection.QueryAsync<NumericDictionary>(query, null);

            return article11;
        }

        private string GetAllQuery()
        {
            return @"Select 
                    	Id,
                    	Title
                    From [Aban360].ClaimPool.SiphonDiameter";
        }
    }
}
