using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class ServiceLinkRawItemsDetailQueryService : ServiceLinkNetRawItemsBase, IServiceLinkRawItemsDetailQueryService
    {
        public ServiceLinkRawItemsDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawNetItemsDetailDataOutputDto>> Get(ServiceLinkRawItemsInputDto input)
        {
            string rawCondition = @"AND	r.TypeCode IN (1,2)";
            string query = GetDetailQuery(rawCondition);

            IEnumerable<ServiceLinkRawNetItemsDetailDataOutputDto> data = await _sqlReportConnection.QueryAsync<ServiceLinkRawNetItemsDetailDataOutputDto>(query, input);
            ServiceLinkRawItemsHeaderOutputDto collectionBranchHeader = new ServiceLinkRawItemsHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (data is not null && data.Any()) ? data.Count() : 0,
                CustomerCount = (data is not null && data.Any()) ? data.Count() : 0,
                Title= ReportLiterals.ServiceLinkRawItemsDetail,

                SumAmount = data.Sum(x => x.Amount),
                SumOffAmount = data.Sum(x => x.OffAmount),
                SumFinalAmount = data.Sum(x => x.FinalAmount),
            };
            var result = new ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawNetItemsDetailDataOutputDto>
                (ReportLiterals.ServiceLinkRawItemsDetail, collectionBranchHeader, data);

            return result;
        }
    }
}