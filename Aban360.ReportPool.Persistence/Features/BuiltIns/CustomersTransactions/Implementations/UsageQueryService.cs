using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class UsageQueryService : AbstractBaseConnection, IUsageQueryService
    {
        public UsageQueryService(IConfiguration configuration)
            : base(configuration)
        { }
        public async Task<ReportOutput<UsageHeaderOutputDto, UsageDataOutputDto>> GetInfo(UsageInputDto input)
        {
            string usageQuery = GetUsageQuery();
            IEnumerable<UsageDataOutputDto> usageData = await _sqlReportConnection.QueryAsync<UsageDataOutputDto>(usageQuery);//todo: params
            UsageHeaderOutputDto usageHeader = new UsageHeaderOutputDto()
            { };

            var result = new ReportOutput<UsageHeaderOutputDto, UsageDataOutputDto>(ReportLiterals.Usage, usageHeader, usageData);

            return result;
        }

        private string GetUsageQuery()
        {
            return @"";
        }
    }
}
