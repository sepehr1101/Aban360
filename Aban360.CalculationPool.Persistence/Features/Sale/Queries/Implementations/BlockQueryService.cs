using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Queries.Implementations
{
    internal sealed class BlockQueryService : AbstractBaseConnection, IBlockQueryService
    {
        public BlockQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IEnumerable<BlockGetDto>> Get()
        {
            string query = GetAllQuery();
            IEnumerable<BlockGetDto> article11 = await _sqlConnection.QueryAsync<BlockGetDto>(query, null);

            return article11;
        }

        private string GetAllQuery()
        {
            return @"Select Distinct BlockCode
                    From [Aban360].CalculationPool.Article11
                    Where BlockCode IS NOT NULL";
        }
    }
}
