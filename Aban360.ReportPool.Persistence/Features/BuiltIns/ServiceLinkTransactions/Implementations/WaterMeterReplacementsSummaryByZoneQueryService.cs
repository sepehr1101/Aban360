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
    internal sealed class WaterMeterReplacementsSummaryByZoneQueryService : WaterMeterReplacementsBase, IWaterMeterReplacementsSummaryByZoneQueryService
    {
        public WaterMeterReplacementsSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsSummaryDataOutputDto>> Get(WaterMeterReplacementsInputDto input)
        {
            string query = GetGroupedWithCteQuery(input.IsChangeDate, "c.ZoneTitle");            
            string reportTitle = input.IsChangeDate == true ? ReportLiterals.WaterMeterReplacements(ReportLiterals.ChangeDate) + ReportLiterals.ByZone : ReportLiterals.WaterMeterReplacements(ReportLiterals.RegisterDate) + ReportLiterals.ByZone;

            IEnumerable<WaterMeterReplacementsSummaryDataOutputDto> waterMeterReplacementsData = await _sqlReportConnection.QueryAsync<WaterMeterReplacementsSummaryDataOutputDto>(query, input);
            WaterMeterReplacementsHeaderOutputDto waterMeterReplacementsHeader = new WaterMeterReplacementsHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = waterMeterReplacementsData is not null && waterMeterReplacementsData.Any() ? waterMeterReplacementsData.Count() : 0,
                Title = reportTitle,

                CustomerCount = waterMeterReplacementsData.Sum(i => i.CustomerCount),
                SumCommercialUnit = waterMeterReplacementsData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = waterMeterReplacementsData.Sum(i => i.DomesticUnit),
                SumOtherUnit = waterMeterReplacementsData.Sum(i => i.OtherUnit),
                TotalUnit = waterMeterReplacementsData.Sum(i => i.TotalUnit),
            };
            var result = new ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsSummaryDataOutputDto>(
                   reportTitle,
                   waterMeterReplacementsHeader,
                   waterMeterReplacementsData);
            return result;
        }
    }
}
