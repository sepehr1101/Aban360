using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class LinkServiceStatementQueryService : AbstractBaseConnection, ILinkServiceStatementQueryService
    {
        public LinkServiceStatementQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<LinkServiceStatementHeaderOutputDto, LinkServiceStatementDataOutputDto>> GetInfo(LinkServiceStatementInputDto input)
        {
            string linkServiceStatementDataInfoQuery = GetLinkServiceStatementDataQuery();
            IEnumerable<LinkServiceStatementDataOutputDto> linkServiceStatementData = await _sqlConnection.QueryAsync<LinkServiceStatementDataOutputDto>(linkServiceStatementDataInfoQuery);//todo: send parameters
            LinkServiceStatementHeaderOutputDto linkServiceStatementHeader = new LinkServiceStatementHeaderOutputDto()
            { };

            var result = new ReportOutput<LinkServiceStatementHeaderOutputDto, LinkServiceStatementDataOutputDto>(ReportLiterals.LinkServiceStatement, linkServiceStatementHeader, linkServiceStatementData);
            return result;
        }

        private string GetLinkServiceStatementDataQuery()
        {
            return " ";
        }
    }
}