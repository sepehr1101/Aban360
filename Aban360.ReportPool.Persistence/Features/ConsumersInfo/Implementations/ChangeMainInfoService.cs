using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class ChangeMainInfoService : AbstractBaseConnection, IChangeMainInfoService
    {
        public ChangeMainInfoService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<IEnumerable<ChangeMainInfoDto>> GetInfo(string billId)
        {
            string ChangeMainQuery = GetChangeMainSummayDtoQuery();
            IEnumerable<ChangeMainInfoDto> result = await _sqlConnection.QueryAsync<ChangeMainInfoDto>(ChangeMainQuery, new { billId });

            return result;
        }
        private string GetChangeMainSummayDtoQuery()
        {
            return @"";
        }
    }
}
