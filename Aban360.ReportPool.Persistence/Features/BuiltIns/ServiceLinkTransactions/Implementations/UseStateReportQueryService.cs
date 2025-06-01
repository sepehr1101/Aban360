using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class UseStateReportQueryService : AbstractBaseConnection, IUseStateReportQueryService
    {
        public UseStateReportQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ICollection<UseStateReportOutputDto>> GetInfo(UseStateReportInputDto input)
        {
            string useStateQueryString = GetUseStateQuery();
            IEnumerable<UseStateReportOutputDto> result = await _sqlConnection.QueryAsync<UseStateReportOutputDto>(useStateQueryString, new { input.UseStateId, input.FromDate, input.ToDate });

            return result.ToList();
        }

        private string GetUseStateQuery()
        {
            return " ";
        }
    }
}
