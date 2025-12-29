using Aban360.Common.BaseEntities;
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
    internal sealed class MalfunctionMeterByDurationGrowthSummaryByZoneQueryService : MalfunctionByDurationBase, IMalfunctionMeterByDurationGrowthSummaryByZoneQueryService
    {
        public MalfunctionMeterByDurationGrowthSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryByZoneDataOutputDto>> Get(MalfunctionMeterByDurationGrowthInputDto input)
        {
            string registerDateBillCondition = $@" AND b.RegisterDay <=@BaseDateJalali ";
            string changeDateJalaliCondition = $@" AND mc.ChangeDateJalali <= @BaseDateJalali ";
            string malfunctionMeterByDurationQueryString = GetGroupedQueryLatest(true, registerDateBillCondition,changeDateJalaliCondition);
            string reportTitle = ReportLiterals.MalfunctionMeterByDurationGrowthSummary + ReportLiterals.ByZone;
            var @params = new 
            {
                FromReadingNumber=input.FromReadingNumber,
                ToReadingNumber=input.ToReadingNumber,
                FromMalfunctionPeriodCount=input.FromMalfunctionPeriodCount,
                ToMalfunctionPeriodCount=input.ToMalfunctionPeriodCount,
                registerDateBill=input.BaseDateJalali,
                changeDateJalali=input.BaseDateJalali,
                zoneIds=input.ZoneIds
            };

            IEnumerable<MalfunctionMeterByDurationSummaryByZoneDataOutputDto> malfunctionMeterByDurationData = await _sqlReportConnection.QueryAsync<MalfunctionMeterByDurationSummaryByZoneDataOutputDto>(malfunctionMeterByDurationQueryString, input, null, 180);
            MalfunctionMeterByDurationHeaderOutputDto malfunctionMeterByDurationHeader = new MalfunctionMeterByDurationHeaderOutputDto()
            {
                Title=reportTitle,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromMalfunctionPeriodCount = input.FromMalfunctionPeriodCount,
                ToMalfunctionPeriodCount = input.ToMalfunctionPeriodCount,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = malfunctionMeterByDurationData is not null && malfunctionMeterByDurationData.Any() ? malfunctionMeterByDurationData.Count() : 0,

                CustomerCount = malfunctionMeterByDurationData?.Sum(r => r.CustomerCount) ?? 0,
                SumCommercialUnit = malfunctionMeterByDurationData?.Sum(r => r.CommercialUnit) ?? 0,
                SumDomesticUnit = malfunctionMeterByDurationData?.Sum(r => r.DomesticUnit) ?? 0,
                SumOtherUnit = malfunctionMeterByDurationData?.Sum(r => r.OtherUnit) ?? 0,
                TotalUnit = malfunctionMeterByDurationData?.Sum(r => r.TotalUnit) ?? 0,
            };

            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryByZoneDataOutputDto> result = new ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryByZoneDataOutputDto>(reportTitle, malfunctionMeterByDurationHeader, malfunctionMeterByDurationData);
            return result;
        }
    }
}
