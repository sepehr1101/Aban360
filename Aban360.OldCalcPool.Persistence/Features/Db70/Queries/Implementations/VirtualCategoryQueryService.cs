using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Implementations
{
    internal sealed class VirtualCategoryQueryService : AbstractBaseConnection, IVirtualCategoryQueryService
    {
        public VirtualCategoryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<VirtualCategoryGetDto> Get(SearchShortInputDto input)
        {
            string query = GetSingleQuery();
            VirtualCategoryGetDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<VirtualCategoryGetDto>(query, input);

            return result;
        }

        public async Task<IEnumerable<VirtualCategoryGetDto>> Get()
        {
            string query = GetAllQuery();
            IEnumerable<VirtualCategoryGetDto> result = await _sqlReportConnection.QueryAsync<VirtualCategoryGetDto>(query, null);

            return result;
        }

        private string GetSingleQuery()
        {
            return @"Select *
                    From [Db70].dbo.VirtualCategory
                    Where Id=@Id";
        }
        private string GetAllQuery()
        {
            return @"Select *
                    From [Db70].dbo.VirtualCategory";
        }
    }
}
