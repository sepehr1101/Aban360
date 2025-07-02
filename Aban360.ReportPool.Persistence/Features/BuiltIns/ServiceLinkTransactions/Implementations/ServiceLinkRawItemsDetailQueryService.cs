using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class ServiceLinkRawItemsDetailQueryService : AbstractBaseConnection, IServiceLinkRawItemsDetailQueryService
    {
        public ServiceLinkRawItemsDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawItemsDetailDataOutputDto>> Get(ServiceLinkRawItemsInputDto input)
        {
            string serviceLinkRawItemsDetailQuery = GetServiceLinkRawItemsDetailQuery(); ;

            var @params = new
            {
            };
            IEnumerable<ServiceLinkRawItemsDetailDataOutputDto> CollectionBranchData = await _sqlReportConnection.QueryAsync<ServiceLinkRawItemsDetailDataOutputDto>(serviceLinkRawItemsDetailQuery, @params);
            ServiceLinkRawItemsHeaderOutputDto CollectionBranchHeader = new ServiceLinkRawItemsHeaderOutputDto()
            {
            };
            var result = new ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawItemsDetailDataOutputDto>
                (ReportLiterals.ServiceLinkRawItemsDetail, CollectionBranchHeader, CollectionBranchData);

            return result;
        }
        private string GetServiceLinkRawItemsDetailQuery()
        {
            return @"";
        }
    }
}
