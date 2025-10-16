using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class UnreadSummaryByZoneQueryService : UnreadBase, IUnreadSummaryByZoneQueryService
    {
        public UnreadSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryByZoneDataOutputDto>> GetInfo(UnreadInputDto input)
        {
            string reportTitle = ReportLiterals.UnreadSummary + ReportLiterals.ByZone;
            string query = GetGroupedQuery(input.ZoneIds.HasValue(),GroupingFields.ZoneTitle);
            
            IEnumerable<UnreadSummaryByZoneDataOutputDto> UnreadSummaryByZoneData = await _sqlReportConnection.QueryAsync<UnreadSummaryByZoneDataOutputDto>(query, input, null, 180);
            UnreadSummaryHeaderOutputDto UnreadSummaryByZoneHeader = new UnreadSummaryHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromPeriodCount = input.FromPeriodCount,
                ToPeriodCount = input.ToPeriodCount,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = UnreadSummaryByZoneData is not null && UnreadSummaryByZoneData.Any() ? UnreadSummaryByZoneData.Count() : 0,
                Title=reportTitle,

                SumBarrier = UnreadSummaryByZoneData.Sum(u => u.Barrier),
                SumClosed = UnreadSummaryByZoneData.Sum(u => u.Closed),
                SumCommercialUnit = UnreadSummaryByZoneData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = UnreadSummaryByZoneData.Sum(i => i.DomesticUnit),
                SumOtherUnit = UnreadSummaryByZoneData.Sum(i => i.OtherUnit),
                TotalUnit = UnreadSummaryByZoneData.Sum(i => i.TotalUnit),
                CustomerCount = UnreadSummaryByZoneData.Sum(i => i.CustomerCount),
                Count0 = UnreadSummaryByZoneData?.Sum(i => i.Count0) ?? 0,
                Count1 = UnreadSummaryByZoneData?.Sum(i => i.Count1) ?? 0,
                Count2 = UnreadSummaryByZoneData?.Sum(i => i.Count2) ?? 0,
                CountMore = UnreadSummaryByZoneData?.Sum(i => i.CountMore) ?? 0,
            };

            var result = new ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryByZoneDataOutputDto>(reportTitle, UnreadSummaryByZoneHeader, UnreadSummaryByZoneData);
            return result;
        }
    }
}
