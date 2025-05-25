using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class ViolationInfoService : AbstractBaseConnection, IViolationInfoService
    {
        public ViolationInfoService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<IEnumerable<ViolationInfoDto>> GetInfo(string billId)
        {
            string ViolationQuery = GetViolationSummayDtoQuery();
            IEnumerable<ViolationInfoDto> result = await _sqlConnection.QueryAsync<ViolationInfoDto>(ViolationQuery, new { billId });

            return result;
        }
        private string GetViolationSummayDtoQuery()
        {
            return @"";
        }
    }
}
