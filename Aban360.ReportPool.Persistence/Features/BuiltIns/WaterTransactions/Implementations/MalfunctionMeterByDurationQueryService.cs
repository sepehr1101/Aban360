using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class MalfunctionMeterByDurationQueryService : AbstractBaseConnection, IMalfunctionMeterByDurationQueryService
    {
        public MalfunctionMeterByDurationQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationDataOutputDto>> Get(MalfunctionMeterByDurationInputDto input)
        {
            string malfunctionMeterByDurationQueryString = GetMalfunctionMeterByDurationQuery();
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<MalfunctionMeterByDurationDataOutputDto> malfunctionMeterByDurationData = await _sqlReportConnection.QueryAsync<MalfunctionMeterByDurationDataOutputDto>(malfunctionMeterByDurationQueryString, @params);
            MalfunctionMeterByDurationHeaderOutputDto malfunctionMeterByDurationHeader = new MalfunctionMeterByDurationHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = malfunctionMeterByDurationData.Count()
            };

            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationDataOutputDto> result = new ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationDataOutputDto>(ReportLiterals.MalfunctionMeterByDuration, malfunctionMeterByDurationHeader, malfunctionMeterByDurationData);
            return result;
        }

        private string GetMalfunctionMeterByDurationQuery()
        {
            return @"";
        }
    }
}
