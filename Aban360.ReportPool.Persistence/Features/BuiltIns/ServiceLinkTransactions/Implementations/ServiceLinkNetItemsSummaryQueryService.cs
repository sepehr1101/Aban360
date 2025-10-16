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
    internal sealed class ServiceLinkNetItemsSummaryQueryService : ServiceLinkNetRawItemsBase, IServiceLinkNetItemsSummaryQueryService
    {
        public ServiceLinkNetItemsSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<ServiceLinkNetItemsHeaderOutputDto, ServiceLinkRawNetItemsSummaryDataOutputDto>> Get(ServiceLinkNetItemsInputDto input)
        {
            string query = GetGroupedQuery(string.Empty);

            IEnumerable<ServiceLinkRawNetItemsSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<ServiceLinkRawNetItemsSummaryDataOutputDto>(query, input);
            ServiceLinkNetItemsHeaderOutputDto header = new ServiceLinkNetItemsHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (data is not null && data.Any()) ? data.Count() : 0,
                CustomerCount = (data is not null && data.Any()) ? data.Count() : 0,
                Title= ReportLiterals.ServiceLinkNetItemsSummary,

                SumAmount = data.Sum(x => x.Amount),
                SumOffAmount = data.Sum(x => x.OffAmount),
                SumFinalAmount = data.Sum(x => x.FinalAmount),
            };
            var result = new ReportOutput<ServiceLinkNetItemsHeaderOutputDto, ServiceLinkRawNetItemsSummaryDataOutputDto>
                (ReportLiterals.ServiceLinkNetItemsSummary, header, data);

            return result;
        }
    }
}
