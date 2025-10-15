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
    internal sealed class MalfunctionMeterByDurationSummaryByUsageQueryService : MalfunctionByDurationBase, IMalfunctionMeterByDurationSummaryByUsageQueryService
    {
        public MalfunctionMeterByDurationSummaryByUsageQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryDataOutputDto>> Get(MalfunctionMeterByDurationInputDto input)
        {
            string malfunctionMeterByDurationQueryString = input.IsMalfunctionLatest ? GetGroupedQueryLatest(false) : GetGroupedQuery(false);
            string reportTitle = ReportLiterals.MalfunctionMeterByDurationSummary + ReportLiterals.ByUsage;
          
            IEnumerable<MalfunctionMeterByDurationSummaryDataOutputDto> malfunctionMeterByDurationData = await _sqlReportConnection.QueryAsync<MalfunctionMeterByDurationSummaryDataOutputDto>(malfunctionMeterByDurationQueryString, input, null, 180);
            MalfunctionMeterByDurationHeaderOutputDto malfunctionMeterByDurationHeader = new MalfunctionMeterByDurationHeaderOutputDto()
            {
                Title = reportTitle,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = malfunctionMeterByDurationData is not null && malfunctionMeterByDurationData.Any() ? malfunctionMeterByDurationData.Count() : 0,

                CustomerCount=malfunctionMeterByDurationData?.Sum(r=>r.CustomerCount)??0
            };

            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryDataOutputDto> result = new ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryDataOutputDto>(reportTitle, malfunctionMeterByDurationHeader, malfunctionMeterByDurationData);
            return result;
        }
    }
}
