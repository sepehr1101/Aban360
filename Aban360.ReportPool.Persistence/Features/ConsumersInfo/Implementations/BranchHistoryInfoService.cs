using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class BranchHistoryInfoService : AbstractBaseConnection, IBranchHistoryInfoService
    {
        public BranchHistoryInfoService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<BranchHistoryInfoDto> GetInfo(string billId)
        {
            string branchHistoryQuery = GetIndividualsSummayDtoQuery();
            BranchHistoryInfoDto result = await _sqlConnection.QueryFirstOrDefaultAsync<BranchHistoryInfoDto>(branchHistoryQuery, new { billId });

            return result;
        }
        private string GetIndividualsSummayDtoQuery()
        {
            return @"";

        }
    
    }
}
