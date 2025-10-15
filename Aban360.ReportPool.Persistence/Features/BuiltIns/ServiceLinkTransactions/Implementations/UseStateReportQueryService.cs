using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class UseStateReportQueryService : UseStateBase, IUseStateReportQueryService
    {
        public UseStateReportQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<UseStateReportHeaderOutputDto, UseStateReportDataOutputDto>> GetInfo(UseStateReportInputDto input)
        {
            string reportTitle = await GetReportTitle(input.UseStateId);
            string query = GetDetailQuery();

            IEnumerable<UseStateReportDataOutputDto> useStateData = await _sqlReportConnection.QueryAsync<UseStateReportDataOutputDto>(query, input);
            UseStateReportHeaderOutputDto useStateHeader = new UseStateReportHeaderOutputDto()
            {
                TotalDebtAmount = useStateData.Sum(useState => useState.DebtAmount),
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                CustomerCount = (useStateData is not null && useStateData.Any()) ? useStateData.Count() : 0,
                RecordCount = (useStateData is not null && useStateData.Any()) ? useStateData.Count() : 0,
                Title = reportTitle,

                SumCommercialUnit = useStateData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = useStateData.Sum(i => i.DomesticUnit),
                SumOtherUnit = useStateData.Sum(i => i.OtherUnit),
                TotalUnit = useStateData.Sum(i => i.TotalUnit)
            };


            var result = new ReportOutput<UseStateReportHeaderOutputDto, UseStateReportDataOutputDto>(reportTitle, useStateHeader, useStateData);

            return result;
        }
        private async Task<string> GetReportTitle(short useStateId)
        {
            string useStateQuery = GetUseStateTitle();
            string useStateTitle = await _sqlConnection.QueryFirstOrDefaultAsync<string>(useStateQuery, new { useStateId = useStateId });

            return ReportLiterals.Report + " " + useStateTitle;
        }

        private string GetUseStateTitle()
        {
            return @"select Title
                     from [Aban360].ClaimPool.UseState 
                     where Id=@useStateId";
        }
    }
}
