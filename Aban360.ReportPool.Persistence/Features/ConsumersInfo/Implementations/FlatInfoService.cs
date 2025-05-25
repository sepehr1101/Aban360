using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class FlatInfoService : AbstractBaseConnection, IFlatInfoService
    {
        public FlatInfoService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<FlatInfoDto> GetInfo(string billId)
        {
            string branchHistoryQuery = GetFlatSummayDtoQuery();
            FlatInfoDto result= await _sqlConnection.QueryFirstOrDefaultAsync<FlatInfoDto>(branchHistoryQuery, new { billId });

            return result;
        }
        private string GetFlatSummayDtoQuery()
        {
            return @"";

        }
    }
}
