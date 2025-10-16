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
    internal sealed class ServiceLinkRawItemsSummaryQueryService : ServiceLinkNetRawItemsBase, IServiceLinkRawItemsSummaryQueryService
    {
        public ServiceLinkRawItemsSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawNetItemsSummaryDataOutputDto>> Get(ServiceLinkRawItemsInputDto input)
        {
            string rawCondition = @"AND	r.TypeCode in (1,2)";
            string query = GetGroupedQuery(rawCondition);

            IEnumerable<ServiceLinkRawNetItemsSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<ServiceLinkRawNetItemsSummaryDataOutputDto>(query, input);
            ServiceLinkRawItemsHeaderOutputDto header = new ServiceLinkRawItemsHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (data is not null && data.Any()) ? data.Count() : 0,
                CustomerCount = (data is not null && data.Any()) ? data.Count() : 0,
                Title= ReportLiterals.ServiceLinkRawItemsSummary,

                SumAmount = data.Sum(x => x.Amount),
                SumOffAmount = data.Sum(x => x.OffAmount),
                SumFinalAmount = data.Sum(x => x.FinalAmount),
            };
            var result = new ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawNetItemsSummaryDataOutputDto>
                (ReportLiterals.ServiceLinkRawItemsSummary, header, data);

            return result;
        }
    }
}
