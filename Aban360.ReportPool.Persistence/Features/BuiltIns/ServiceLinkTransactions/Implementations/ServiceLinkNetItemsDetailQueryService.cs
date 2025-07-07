using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class ServiceLinkNetItemsDetailQueryService : AbstractBaseConnection, IServiceLinkNetItemsDetailQueryService
    {
        public ServiceLinkNetItemsDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<ServiceLinkNetItemsHeaderOutputDto, ServiceLinkNetItemsDetailDataOutputDto>> Get(ServiceLinkNetItemsInputDto input)
        {
            string serviceLinkNetItemsDetailQuery = GetServiceLinkNetItemsDetailQuery(); ;

            var @params = new
            {
            };
            IEnumerable<ServiceLinkNetItemsDetailDataOutputDto> CollectionBranchData = await _sqlReportConnection.QueryAsync<ServiceLinkNetItemsDetailDataOutputDto>(serviceLinkNetItemsDetailQuery, @params);
            ServiceLinkNetItemsHeaderOutputDto CollectionBranchHeader = new ServiceLinkNetItemsHeaderOutputDto()
            {
            };
            var result = new ReportOutput<ServiceLinkNetItemsHeaderOutputDto, ServiceLinkNetItemsDetailDataOutputDto>
                (ReportLiterals.ServiceLinkNetItemsDetail, CollectionBranchHeader, CollectionBranchData);

            return result;
        }
        private string GetServiceLinkNetItemsDetailQuery()
        {
            return @"";
        }
    }
}
