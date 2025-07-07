using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class ServiceLinkNetItemsSummaryQueryService : AbstractBaseConnection, IServiceLinkNetItemsSummaryQueryService
    {
        public ServiceLinkNetItemsSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<ServiceLinkNetItemsHeaderOutputDto, ServiceLinkNetItemsSummaryDataOutputDto>> Get(ServiceLinkNetItemsInputDto input)
        {
            string serviceLinkNetItemsSummaryQuery = GetServiceLinkNetItemsSummaryQuery(); ;

            var @params = new
            {
            };
            IEnumerable<ServiceLinkNetItemsSummaryDataOutputDto> CollectionBranchData = await _sqlReportConnection.QueryAsync<ServiceLinkNetItemsSummaryDataOutputDto>(serviceLinkNetItemsSummaryQuery, @params);
            ServiceLinkNetItemsHeaderOutputDto CollectionBranchHeader = new ServiceLinkNetItemsHeaderOutputDto()
            {
            };
            var result = new ReportOutput<ServiceLinkNetItemsHeaderOutputDto, ServiceLinkNetItemsSummaryDataOutputDto>
                (ReportLiterals.ServiceLinkNetItemsSummary, CollectionBranchHeader, CollectionBranchData);

            return result;
        }
        private string GetServiceLinkNetItemsSummaryQuery()
        {
            return @"";
        }
    }
}
