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
    internal sealed class MalfunctionMeterQueryService : AbstractBaseConnection, IMalfunctionMeterQueryService
    {
        public MalfunctionMeterQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

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
                RecordCount = (malfunctionMeterData is not null && malfunctionMeterData.Any()) ? malfunctionMeterData.Count() : 0,
                TotalPayable = (malfunctionMeterData is not null && malfunctionMeterData.Any()) ? malfunctionMeterData.Sum(x => x.SumItems) : 0
            };
            if (malfunctionMeterData is not null && !malfunctionMeterData.Any())
            {
                malfunctionMeterHeader.TotalPayable = malfunctionMeterData.Sum(x => x.Payable);
            }

            ReportOutput<MalfunctionMeterHeaderOutputDto, MalfunctionMeterDataOutputDto> result = new ReportOutput<MalfunctionMeterHeaderOutputDto, MalfunctionMeterDataOutputDto>(ReportLiterals.MalfunctionMeterDetail, malfunctionMeterHeader, malfunctionMeterData);
            return result;
        }

        private string GetMalfunctionMeterQuery()
        {
            return @"WITH CTE AS (
                    Select
                        b.BillId,
                        b.ZoneTitle,
                        b.CustomerNumber,
                        b.ReadingNumber,
                        b.Duration,
                        b.BranchType,
                        b.RegisterDay AS LastReadingDay,
                        b.Payable,
	                    b.SumItems,
                        b.Consumption,
                        b.ConsumptionAverage,
	                    b.CounterStateCode,
	                    b.CounterStateTitle,
	                    RN=ROW_NUMBER() OVER (PARTITION BY b.BillId ORDER BY b.RegisterDay DESC)
                    From [CustomerWarehouse].dbo.Bills b
					Join [CustomerWarehouse].dbo.Clients c 
						ON b.CustomerNumber=c.CustomerNumber AND b.ZoneId=c.ZoneId
                    Where 
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                        b.ZoneId IN @zoneIds AND
                        b.CounterStateCode NOT IN (4,7,8) AND
						c.DeletionStateId IN (0,2))
                    SELECT * FROM CTE 
                    WHERE RN=1 AND CounterStateCode=1";
        }
    }
}
