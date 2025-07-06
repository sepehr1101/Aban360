using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class ServiceLinkRawItemsSummaryQueryService : AbstractBaseConnection, IServiceLinkRawItemsSummaryQueryService
    {
        public ServiceLinkRawItemsSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawItemsSummaryDataOutputDto>> Get(ServiceLinkRawItemsInputDto input)
        {
            string serviceLinkRawItemsSummaryQuery = GetServiceLinkRawItemsSummaryQuery(); ;

            var @params = new
            {
            };
            IEnumerable<ServiceLinkRawItemsSummaryDataOutputDto> CollectionBranchData = await _sqlReportConnection.QueryAsync<ServiceLinkRawItemsSummaryDataOutputDto>(serviceLinkRawItemsSummaryQuery, @params);
            ServiceLinkRawItemsHeaderOutputDto CollectionBranchHeader = new ServiceLinkRawItemsHeaderOutputDto()
            {
            };
            var result = new ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawItemsSummaryDataOutputDto>
                (ReportLiterals.ServiceLinkRawItemsSummary, CollectionBranchHeader, CollectionBranchData);

            return result;
        }
        private string GetServiceLinkRawItemsSummaryQuery()
        {
            return @"";
        }
    }
}
