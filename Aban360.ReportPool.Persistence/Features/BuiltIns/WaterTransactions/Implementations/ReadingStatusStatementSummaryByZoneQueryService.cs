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
    internal sealed class ReadingStatusStatementSummaryByZoneQueryService : ReadingStatusStatementBase, IReadingStatusStatementSummaryByZoneQueryService
    {
        public ReadingStatusStatementSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementSummaryDataOutputDto>> GetInfo(ReadingStatusStatementInputDto input)
        {
            string reportTitle = ReportLiterals.ReadingStatusStatement + ReportLiterals.ByZone;
            string query = GetGroupedQuery(input.IsRegisterDateJalali, true);

            IEnumerable<ReadingStatusStatementSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<ReadingStatusStatementSummaryDataOutputDto>(query, input);
            ReadingStatusStatementHeaderOutputDto header = new ReadingStatusStatementHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = data is not null && data.Any() ? data.Count() : 0,
                Title = reportTitle,
            };
            if (data is not null && data.Any())
            {
                header.SumClosed = data.Sum(x => x.Closed);
                header.SumObstacle = data.Sum(x => x.Obstacle);
                header.SumPureReading = data.Sum(x => x.PureReading);
                header.SumRuined = data.Sum(x => x.Ruined);
                header.SumTemporarily = data.Sum(x => x.Temporarily);
                header.SumAll = data.Sum(x => x.AllCount);
                header.SumDebt = data.Sum(x => x.Debt);
                header.SumSelfClaimed = data.Sum(x => x.SelfClaimedCount);
            }

            var result = new ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementSummaryDataOutputDto>(reportTitle, header, data);
            return result;
        }
    }
}
