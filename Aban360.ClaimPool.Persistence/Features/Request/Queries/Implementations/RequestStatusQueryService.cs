using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class RequestStatusQueryService : AbstractBaseConnection, IRequestStatusQueryService
    {
        public RequestStatusQueryService(IConfiguration configuration)
            :base(configuration)
        {
        }

        public async Task<IEnumerable<SelectionDto>> GetIsKartable()
        {
            string query = GetIsKartablesQuery();
            IEnumerable<SelectionDto> result=await _sqlReportConnection.QueryAsync<SelectionDto>(query);
            return result;
        }
        private string GetIsKartablesQuery()
        {
            return @"Select 
                    	StatusID Id,
                    	NextTitle Title,
                    	0 IsSelected
                    From AbAndFazelab.dbo.Status
                    Where IsKartable=1";
        }
    }
}
