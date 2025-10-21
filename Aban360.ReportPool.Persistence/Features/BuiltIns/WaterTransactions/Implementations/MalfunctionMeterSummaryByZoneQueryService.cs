using Aban360.Common.BaseEntities;
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
    internal sealed class MalfunctionMeterSummaryByZoneQueryService : MalfunctionMeterBase, IMalfunctionMeterSummaryByZoneQueryService
    {
        public MalfunctionMeterSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, MalfunctionMeterSummaryDataOutputDto>> Get(MalfunctionMeterInputDto input)
        {
            string reportTitle = ReportLiterals.MalfunctionMeterSummary + ReportLiterals.ByZone;
            string query = GetGroupedQueryNonDuplicate(GroupingFields.UsageTitle);//GetGroupedQuery(GroupingFields.ZoneTitle);

            IEnumerable<MalfunctionMeterSummaryDataOutputDto> malfunctionMeterData = await _sqlReportConnection.QueryAsync<MalfunctionMeterSummaryDataOutputDto>(query, input);
            MalfunctionMeterSummaryHeaderOutputDto malfunctionMeterHeader = new MalfunctionMeterSummaryHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = malfunctionMeterData is not null && malfunctionMeterData.Any() ? malfunctionMeterData.Count() : 0,
                Title=reportTitle,

                TotalPayable = malfunctionMeterData is not null && malfunctionMeterData.Any() ? malfunctionMeterData.Sum(x => x.SumItems) : 0,
                ConsumptionAverage = malfunctionMeterData is not null && malfunctionMeterData.Any() ? malfunctionMeterData.Average(x => x.Consumption) : 0,
                SumCommercialUnit = malfunctionMeterData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = malfunctionMeterData.Sum(i => i.DomesticUnit),
                SumOtherUnit = malfunctionMeterData.Sum(i => i.OtherUnit),
                TotalUnit = malfunctionMeterData.Sum(i => i.TotalUnit),
                CustomerCount = malfunctionMeterData.Sum(i => i.CustomerCount)
            };

            ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, MalfunctionMeterSummaryDataOutputDto> result = new ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, MalfunctionMeterSummaryDataOutputDto>(reportTitle, malfunctionMeterHeader, malfunctionMeterData);
            return result;
        }
    }
}
