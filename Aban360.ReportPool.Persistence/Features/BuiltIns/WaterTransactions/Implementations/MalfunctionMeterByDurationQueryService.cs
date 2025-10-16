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
    internal sealed class MalfunctionMeterByDurationQueryService : MalfunctionByDurationBase, IMalfunctionMeterByDurationQueryService
    {
        public MalfunctionMeterByDurationQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationDataOutputDto>> Get(MalfunctionMeterByDurationInputDto input)
        {
            string query = input.IsMalfunctionLatest? GetDetailsQueryLatest():GetDetailsQuery();
            
            IEnumerable<MalfunctionMeterByDurationDataOutputDto> malfunctionMeterByDurationData = await _sqlReportConnection.QueryAsync<MalfunctionMeterByDurationDataOutputDto>(query, input, null, 180);
            MalfunctionMeterByDurationHeaderOutputDto malfunctionMeterByDurationHeader = new MalfunctionMeterByDurationHeaderOutputDto()
            {
                Title= ReportLiterals.MalfunctionMeterByDurationDetail,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (malfunctionMeterByDurationData is not null && malfunctionMeterByDurationData.Any()) ? malfunctionMeterByDurationData.Count() : 0,
                CustomerCount = (malfunctionMeterByDurationData is not null && malfunctionMeterByDurationData.Any()) ? malfunctionMeterByDurationData.Count() : 0,
            };

            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationDataOutputDto> result = new ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationDataOutputDto>(ReportLiterals.MalfunctionMeterByDurationDetail, malfunctionMeterByDurationHeader, malfunctionMeterByDurationData);
            return result;
        }
    }
}
