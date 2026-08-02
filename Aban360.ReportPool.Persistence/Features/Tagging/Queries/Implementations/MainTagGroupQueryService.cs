using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Implementations
{
    internal sealed class MainTagGroupQueryService : AbstractBaseConnection, IMainTagGroupQueryService
    {
        public MainTagGroupQueryService(IConfiguration configuration)
                : base(configuration)
        {
        }

        public async Task<IEnumerable<MainTagGroupGetDto>> GetValid()
        {
            IEnumerable<MainTagGroupGetDto> data = await _sqlReportConnection.QueryAsync<MainTagGroupGetDto>(GetValidQuery());
            return data;
        }
        public async Task<MainTagGroupGetDto> GetValid(int id)
        {
            MainTagGroupGetDto data = await _sqlReportConnection.QueryFirstOrDefaultAsync<MainTagGroupGetDto>(GetValidByIdQuery(), new { id });
            return data;
        }
      
        private string GetValidQuery()
        {
            return @"Select 
                    	Id,
                    	Title,
                    	CreateDateTime
                    From CustomerWarehouse.dbo.MainTagGroup
                    Where DeleteDateTime IS NULL";
        }
        private string GetValidByIdQuery()
        {
            return @"Select 
                    	Id,
                    	Title,
                    	CreateDateTime
                    From CustomerWarehouse.dbo.MainTagGroup
                    Where 
                        Id = @Id AND
                        DeleteDateTime IS NULL";
        }
    }
}
