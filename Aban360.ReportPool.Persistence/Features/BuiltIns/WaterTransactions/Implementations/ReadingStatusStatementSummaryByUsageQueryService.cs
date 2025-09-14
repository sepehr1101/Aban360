using Aban360.Common.BaseEntities;
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
    internal sealed class ReadingStatusStatementSummaryByUsageQueryService : AbstractBaseConnection, IReadingStatusStatementSummaryByUsageQueryService
    {
        public ReadingStatusStatementSummaryByUsageQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementSummaryDataOutputDto>> GetInfo(ReadingStatusStatementInputDto input)
        {
            string readingStatusStatements = GetReadingStatusStatementQuery();
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                fromDate = input.FromDateJalali,
                todate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
                isRegisterDate = input.IsRegisterDateJalali
            };
            IEnumerable<ReadingStatusStatementSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<ReadingStatusStatementSummaryDataOutputDto>(readingStatusStatements, @params);
            ReadingStatusStatementHeaderOutputDto header = new ReadingStatusStatementHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = data is not null && data.Any() ? data.Count() : 0,
            };
            if (data is not null && data.Any())
            {
                header.SumClosed = data.Sum(x => x.Closed);
                header.SumObstacle = data.Sum(x => x.Obstacle);
                header.SumReadingNet = data.Sum(x => x.ReadingNet);
                header.SumRuined = data.Sum(x => x.ReadingNet);
                header.SumTemporarily = data.Sum(x => x.Temporarily);
                header.SumAll = data.Sum(x => x.AllCount);
            }

            var result = new ReportOutput<ReadingStatusStatementHeaderOutputDto, ReadingStatusStatementSummaryDataOutputDto>(ReportLiterals.ReadingStatusStatementSummary + ReportLiterals.ByUsage, header, data);
            return result;
        }

        private string GetReadingStatusStatementQuery()
        {
            return @"Select 
                    	Max(b.UsageTitle) AS ItemTitle,
                    	COUNT(Case When b.CounterStateCode NOT IN (1,4,7,8) Then 1 End)AS ReadingNet,
                    	COUNT(Case When b.CounterStateCode=4 Then 1 End)AS Closed,
                    	COUNT(Case When b.CounterStateCode=7 Then 1 End)AS Obstacle,
                    	COUNT(Case When b.CounterStateCode=8 Then 1 End)AS Temporarily,
                    	COUNT(Case When b.CounterStateCode!=1 Then 1 End)AS AllCount,
                    	COUNT(Case When b.CounterStateCode=1 Then 1 End)AS Ruined
                    From [CustomerWarehouse].dbo.Bills b
                    Where
                    	(
                    	(@isRegisterDate=1 AND b.RegisterDay BETWEEN @fromDate AND @toDate)OR
                    	(@isRegisterDate=0 AND b.NextDay BETWEEN @fromDate AND @toDate)
                    	)AND
                    	(b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber)AND
                    	b.ZoneId in @zoneIds
                    Group By  b.UsageTitle";
        }
    }
}
