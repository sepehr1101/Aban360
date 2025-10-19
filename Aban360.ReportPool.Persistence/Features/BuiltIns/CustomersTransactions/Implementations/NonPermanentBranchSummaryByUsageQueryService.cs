using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class NonPermanentBranchSummaryByUsageQueryService : NonPermanentBranchBase, INonPermanentBranchSummaryByUsageQueryService
    {
        public NonPermanentBranchSummaryByUsageQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchGroupedDataOutputDto>> GetInfo(NonPermanentBranchByUsageAndZoneInputDto input)
        {
            string query = GetGroupedQuery($"c.{GroupingFields.UsageTitle}");

            IEnumerable<NonPermanentBranchGroupedDataOutputDto> nonPremanentBranchData = await _sqlReportConnection.QueryAsync<NonPermanentBranchGroupedDataOutputDto>(query, input);
            NonPermanentBranchHeaderOutputDto nonPremanentBranchHeader = new NonPermanentBranchHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = nonPremanentBranchData is not null && nonPremanentBranchData.Any() ? nonPremanentBranchData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = ReportLiterals.NonPermanentBranchSummary + ReportLiterals.ByUsage,

                CustomerCount = nonPremanentBranchData.Sum(i => i.CustomerCount),
                SumCommercialUnit = nonPremanentBranchData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = nonPremanentBranchData.Sum(i => i.DomesticUnit),
                SumOtherUnit = nonPremanentBranchData.Sum(i => i.OtherUnit),
                TotalUnit = nonPremanentBranchData.Sum(i => i.TotalUnit)
            };

            var result = new ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchGroupedDataOutputDto>(ReportLiterals.NonPermanentBranchSummary + ReportLiterals.ByUsage, nonPremanentBranchHeader, nonPremanentBranchData);

            return result;
        }
    }
}
