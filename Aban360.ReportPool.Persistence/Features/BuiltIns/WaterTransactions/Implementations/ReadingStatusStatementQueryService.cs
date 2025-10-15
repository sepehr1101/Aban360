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
    internal sealed class ReadingStatusStatementQueryService : ReadingStatusStatementBase, IReadingStatusStatementQueryService
    {
        public ReadingStatusStatementQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementDataOutputDto>> GetInfo(ReadingStatusStatementInputDto input)
        {
            string query = GetDetailQuery(input.IsRegisterDateJalali);

            IEnumerable<ReadingStatusStatementDataOutputDto> data = await _sqlReportConnection.QueryAsync<ReadingStatusStatementDataOutputDto>(query, input);
            ReadingStatusStatementHeaderOutputDto header = new ReadingStatusStatementHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (data is not null && data.Any()) ? data.Count() : 0,
                Title = ReportLiterals.ReadingStatusStatement,
            };
            if (data is not null && data.Any())
            {
                header.SumClosed = data.Sum(x => x.Closed);
                header.SumObstacle = data.Sum(x => x.Obstacle);
                header.SumPureReading = data.Sum(x => x.PureReading);
                header.SumRuined = data.Sum(x => x.Ruined);
                header.SumTemporarily = data.Sum(x => x.Temporarily);
                header.SumAll = data.Sum(x => x.AllCount);
                header.SumSelfClaimed = data.Sum(x => x.SelfClaimedCount);
                header.SumDebt = data.Sum(x => x.Debt);
            }

            var result = new ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementDataOutputDto>(ReportLiterals.ReadingStatusStatement, header, data);
            return result;
        }
    }
}
