using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WithoutBillSummaryByZoneQueryService : WithoutBillBase, IWithoutBillSummaryByZoneQueryService
    {
        public WithoutBillSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<WithoutBillHeaderOutputDto, WithoutBillSummaryDataOutputDto>> GetInfo(WithoutBillInputDto input)
        {
            string reportTitle = ReportLiterals.WithoutBill + ReportLiterals.ByZone;
            string query = GetGroupedQuery(input.ZoneIds.HasValue(), input.UsageIds.HasValue(), true);

            IEnumerable<WithoutBillSummaryDataOutputDto> withoutBillData = await _sqlReportConnection.QueryAsync<WithoutBillSummaryDataOutputDto>(query, input);
            WithoutBillHeaderOutputDto withoutBillHeader = new WithoutBillHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = withoutBillData is not null && withoutBillData.Any() ? withoutBillData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title=reportTitle,

                SumCommercialUnit = withoutBillData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = withoutBillData.Sum(i => i.DomesticUnit),
                SumOtherUnit = withoutBillData.Sum(i => i.OtherUnit),
                TotalUnit = withoutBillData.Sum(i => i.TotalUnit),
                CustomerCount = withoutBillData.Sum(i => i.CustomerCount),
            };

            var result = new ReportOutput<WithoutBillHeaderOutputDto, WithoutBillSummaryDataOutputDto>(reportTitle, withoutBillHeader, withoutBillData);
            return result;
        }
    }
}
