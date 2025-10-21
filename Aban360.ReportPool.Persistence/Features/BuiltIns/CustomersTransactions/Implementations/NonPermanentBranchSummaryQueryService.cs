using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class NonPermanentBranchSummaryQueryService : NonPermanentBranchBase, INonPermanentBranchSummaryQueryService
    {
        public NonPermanentBranchSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryDataOutputDto>> GetInfo(NonPermanentBranchInputDto input)
        {
            string query = GetSummaryQuery();

            IEnumerable<NonPermanentBranchSummaryDataOutputDto> nonPremanentBranchData = await _sqlReportConnection.QueryAsync<NonPermanentBranchSummaryDataOutputDto>(query, input);
            NonPermanentBranchHeaderOutputDto nonPremanentBranchHeader = new NonPermanentBranchHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = (nonPremanentBranchData is not null && nonPremanentBranchData.Any()) ? nonPremanentBranchData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = ReportLiterals.NonPermanentBranchSummary + ReportLiterals.ByUsageAndZoneAndDiameter,
                CustomerCount = nonPremanentBranchData?.Sum(x => x.Count) ?? 0,

                SumCommercialUnit = nonPremanentBranchData?.Sum(x => x.CommercialUnit) ?? 0,
                SumDomesticUnit = nonPremanentBranchData?.Sum(x => x.DomesticUnit) ?? 0,
                SumOtherUnit = nonPremanentBranchData?.Sum(x => x.OtherUnit) ?? 0,
                TotalUnit = nonPremanentBranchData?.Sum(x => x.TotalUnit) ?? 0,
            };

            var result = new ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryDataOutputDto>(ReportLiterals.NonPermanentBranchSummary + ReportLiterals.ByUsageAndZoneAndDiameter, nonPremanentBranchHeader, nonPremanentBranchData);

            return result;
        }
    }
}
