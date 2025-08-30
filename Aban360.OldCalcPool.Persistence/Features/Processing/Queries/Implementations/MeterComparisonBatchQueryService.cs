using Aban360.Common.Db.Dapper;
using Aban360.Common.Excel;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Implementations
{
    internal sealed class MeterComparisonBatchQueryService : AbstractBaseConnection, IMeterComparisonBatchQueryService
    {
        string reportTitle = "مغایرت محاسبه";
        public MeterComparisonBatchQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<MeterComparisonBatchHeaderOutputDto, MeterComparisonBatchDataOutputDto>> Get(MeterComparisonBatchInputDto input)
        {
            string perviousBillsDataQueryString = GetPreviousBillsDataQuery();
            var @params = new
            {
                fromDateJalali = input.FromDateJalali,
                toDateJalali = input.ToDateJalali,
                zoneId = input.ZoneId,
            };
            IEnumerable<MeterComparisonBatchDataOutputDto> details = await _sqlReportConnection.QueryAsync<MeterComparisonBatchDataOutputDto>(perviousBillsDataQueryString, @params);
            MeterComparisonBatchHeaderOutputDto summary = new()
            {
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = details.Count(),
                ZoneTitle = details.FirstOrDefault().ZoneTitle,
                SumPreviousAmount = details.Sum(m => m.PreviousAmount),
            };
            ReportOutput<MeterComparisonBatchHeaderOutputDto, MeterComparisonBatchDataOutputDto> result = new(reportTitle, summary, details);
            return result;
        }

        private string GetPreviousBillsDataQuery()
        {
            return @"use CustomerWarehouse
                    Select
                    	b.PreviousDay AS PreviousDateJalali,
                    	b.PreviousNumber AS PreviousMeterNumber,
                    	b.NextDay AS CurrentDateJalali,
                    	b.NextNumber AS CurrentMeterNumber,
                    	TRIM(b.BillId) AS BillId,
                        b.ZoneTitle AS ZoneTitle,
                    	b.SumItems AS PreviousAmount
                    From [CustomerWarehouse].dbo.Bills b
                    Where
                    	b.ZoneId=@zoneId AND
                    	b.RegisterDay BETWEEN @fromDateJalali AND @toDateJalali";
        }
    }
}
