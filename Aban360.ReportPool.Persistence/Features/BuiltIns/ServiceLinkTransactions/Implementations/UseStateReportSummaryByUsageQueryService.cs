using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class UseStateReportSummaryByUsageQueryService : UseStateBase, IUseStateReportSummaryByUsageQueryService
    {
        public UseStateReportSummaryByUsageQueryService(IConfiguration configuration)
            : base(configuration) 
        {
        }

        public async Task<ReportOutput<UseStateReportHeaderSummaryOutputDto, UseStateReportSummaryDataOutputDto>> Get(UseStateReportInputDto input)
        {
            string reportTitle= await GetReportTitle(input.UseStateId);
            string query = GetGroupedQuery(GroupingFields.UsageTitle);

            IEnumerable<UseStateReportSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<UseStateReportSummaryDataOutputDto>(query, input);
            UseStateReportHeaderSummaryOutputDto header = new UseStateReportHeaderSummaryOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = data is not null && data.Any() ? data.Count() : 0,
                Title= reportTitle,

                SumCommercialUnit = data.Sum(i => i.CommercialUnit),
                SumDomesticUnit = data.Sum(i => i.DomesticUnit),
                SumOtherUnit = data.Sum(i => i.OtherUnit),
                TotalUnit = data.Sum(i => i.TotalUnit),
                CustomerCount = data.Sum(i => i.CustomerCount),
            };

            var result = new ReportOutput<UseStateReportHeaderSummaryOutputDto, UseStateReportSummaryDataOutputDto>(reportTitle, header, data);
            return result;
        }

        private async Task<string> GetReportTitle(short useStateId)
        {
            string useStateQuery = GetUseStateTitle();
            string useStateTitle = await _sqlConnection.QueryFirstOrDefaultAsync<string>(useStateQuery, new { useStateId = useStateId });

            return ReportLiterals.Report + " " + ReportLiterals.ByUsage + useStateTitle;
        }
        
        private string GetUseStateTitle()
        {
            return @"select Title
                     from [Aban360].ClaimPool.UseState 
                     where Id=@useStateId";
        }
    }
}
