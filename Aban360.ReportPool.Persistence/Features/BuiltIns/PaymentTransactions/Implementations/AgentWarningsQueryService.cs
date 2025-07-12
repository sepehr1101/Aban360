using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class AgentWarningsQueryService : AbstractBaseConnection, IAgentWarningsQueryService
    {
        public AgentWarningsQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<AgentWarningsHeaderOutputDto, AgentWarningsDataOutputDto>> GetInfo(AgentWarningsInputDto input)
        {
            string AgentWarningss = GetAgentWarningsQuery();
            var @params = new
            { };
            IEnumerable<AgentWarningsDataOutputDto> AgentWarningsData = await _sqlReportConnection.QueryAsync<AgentWarningsDataOutputDto>(AgentWarningss, @params);
            AgentWarningsHeaderOutputDto AgentWarningsHeader = new AgentWarningsHeaderOutputDto()
            { };

            var result = new ReportOutput<AgentWarningsHeaderOutputDto, AgentWarningsDataOutputDto>(ReportLiterals.AgentWarnings, AgentWarningsHeader, AgentWarningsData);
            return result;
        }

        private string GetAgentWarningsQuery()
        {
            return @"";
        }
    }
}
