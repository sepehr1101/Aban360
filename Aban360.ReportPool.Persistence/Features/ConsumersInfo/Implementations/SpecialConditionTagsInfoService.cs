using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class SpecialConditionTagsInfoService: AbstractBaseConnection, ISpecialConditionTagsInfoService
    {
        public SpecialConditionTagsInfoService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<SpecialConditionTagsInfoDto> GetInfo(string billId)
        {
            string branchHistoryQuery = GetSpecialConditionTagsSummayDtoQuery();
            SpecialConditionTagsInfoDto result = await _sqlConnection.QueryFirstOrDefaultAsync<SpecialConditionTagsInfoDto>(branchHistoryQuery, new { billId });

            return result;
        }
        private string GetSpecialConditionTagsSummayDtoQuery()
        {
            return @"";

        }
    }
}
