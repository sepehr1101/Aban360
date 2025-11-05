using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Implementations
{
    internal sealed class WaterDiameterQueryService : AbstractBaseConnection, IWaterDiameterQueryService
    {
        public WaterDiameterQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IEnumerable<NumericDictionary>> Get()
        {
            string query = GetAllQuery();
            IEnumerable<NumericDictionary> article11 = await _sqlReportConnection.QueryAsync<NumericDictionary>(query, null);

            return article11;
        }

        private string GetAllQuery()
        {
            return @"Select 
                    	C1 as Id,
                    	C2 as Title
                    From [Db70].dbo.T5";
        }
    }

}
