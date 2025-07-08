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
    internal sealed class MalfunctionMeterQueryService : AbstractBaseConnection, IMalfunctionMeterQueryService
    {
        public MalfunctionMeterQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<MalfunctionMeterHeaderOutputDto, MalfunctionMeterDataOutputDto>> Get(MalfunctionMeterInputDto input)
        {
            string malfunctionMeterQueryString = GetMalfunctionMeterQuery();
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<MalfunctionMeterDataOutputDto> malfunctionMeterData = await _sqlReportConnection.QueryAsync<MalfunctionMeterDataOutputDto>(malfunctionMeterQueryString, @params);
            MalfunctionMeterHeaderOutputDto malfunctionMeterHeader = new MalfunctionMeterHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = malfunctionMeterData.Count()
            };

            ReportOutput<MalfunctionMeterHeaderOutputDto, MalfunctionMeterDataOutputDto> result = new ReportOutput<MalfunctionMeterHeaderOutputDto, MalfunctionMeterDataOutputDto>(ReportLiterals.MalfunctionMeter, malfunctionMeterHeader, malfunctionMeterData);
            return result;
        }

        private string GetMalfunctionMeterQuery()
        {
            return @"Select
                    	b.BillId,
                    	b.ZoneTitle,
                    	b.CustomerNumber,
                    	b.ReadingNumber,
                    	b.Duration,
                    	b.BranchType,
                    	b.RegisterDay AS LastReadingDay,
                    	b.Payable,
                    	b.Consumption,
                    	b.ConsumptionAverage
                    From [CustomerWarehouse].dbo.Bills b
                    Where 
                    	b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber AND
                    	b.ZoneId IN @zoneIds AND
                    	b.CounterStateCode=4";
        }
    }
}
