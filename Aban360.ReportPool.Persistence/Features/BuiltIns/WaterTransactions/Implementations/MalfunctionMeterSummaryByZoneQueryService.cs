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
    internal sealed class MalfunctionMeterSummaryByZoneQueryService : AbstractBaseConnection, IMalfunctionMeterSummaryByZoneQueryService
    {
        public MalfunctionMeterSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, MalfunctionMeterSummaryDataOutputDto>> Get(MalfunctionMeterInputDto input)
        {
            string malfunctionMeterQueryString = GetMalfunctionMeterQuery();
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<MalfunctionMeterSummaryDataOutputDto> malfunctionMeterData = await _sqlReportConnection.QueryAsync<MalfunctionMeterSummaryDataOutputDto>(malfunctionMeterQueryString, @params);
            MalfunctionMeterSummaryHeaderOutputDto malfunctionMeterHeader = new MalfunctionMeterSummaryHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = malfunctionMeterData is not null && malfunctionMeterData.Any() ? malfunctionMeterData.Count() : 0,

                TotalPayable = malfunctionMeterData is not null && malfunctionMeterData.Any() ? malfunctionMeterData.Sum(x => x.SumItems) : 0,
                ConsumptionAverage = malfunctionMeterData is not null && malfunctionMeterData.Any() ? malfunctionMeterData.Average(x => x.Consumption ) : 0,
                SumCommercialUnit = malfunctionMeterData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = malfunctionMeterData.Sum(i => i.DomesticUnit),
                SumOtherUnit = malfunctionMeterData.Sum(i => i.OtherUnit),
                TotalUnit = malfunctionMeterData.Sum(i => i.TotalUnit),
                CustomerCount = malfunctionMeterData.Sum(i => i.CustomerCount)
            };

            ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, MalfunctionMeterSummaryDataOutputDto> result = new ReportOutput<MalfunctionMeterSummaryHeaderOutputDto, MalfunctionMeterSummaryDataOutputDto>(ReportLiterals.MalfunctionMeterSummary + ReportLiterals.ByZone, malfunctionMeterHeader, malfunctionMeterData);
            return result;
        }

        private string GetMalfunctionMeterQuery()
        {
            return @"WITH CTE AS (
                    Select
                        b.ZoneTitle,
	                    b.SumItems,
                        b.Consumption,
						b.CommercialCount,
						b.DomesticCount,
						b.OtherCount,
						b.WaterDiameterId,
						b.CounterStateCode,
                        b.ConsumptionAverage,
	                    RN=ROW_NUMBER() OVER (PARTITION BY b.BillId ORDER BY b.RegisterDay DESC)
                    From [CustomerWarehouse].dbo.Bills b
					Join [CustomerWarehouse].dbo.Clients c 
						ON b.CustomerNumber=c.CustomerNumber AND b.ZoneId=c.ZoneId
                    Where 
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND                        b.ZoneId IN @zoneIds AND
                        b.CounterStateCode NOT IN (4,7,8) AND
						c.DeletionStateId IN (0,2) AND
						(@fromDate IS NULL OR
						@toDate IS NULL OR
						b.RegisterDay BETWEEN @fromDate AND @toDate))
                    SELECT 
						ZoneTitle as ItemTitle,
						SUM(c.SumItems) as SumItems,
						AVG(c.ConsumptionAverage) as Consumption,
						COUNT(c.ZoneTitle) AS CustomerCount,
						SUM(ISNULL(c.CommercialCount, 0) + ISNULL(c.DomesticCount, 0) + ISNULL(c.OtherCount, 0)) AS TotalUnit,
						SUM(ISNULL(c.CommercialCount, 0)) AS CommercialUnit,
						SUM(ISNULL(c.DomesticCount, 0)) AS DomesticUnit,
						SUM(ISNULL(c.OtherCount, 0)) AS OtherUnit,
						SUM(CASE WHEN t5.C0 = 0 THEN 1 ELSE 0 END) AS UnSpecified,
						SUM(CASE WHEN t5.C0 = 1 THEN 1 ELSE 0 END) AS Field0_5,
						SUM(CASE WHEN t5.C0 = 2 THEN 1 ELSE 0 END) AS Field0_75,
						SUM(CASE WHEN t5.C0 = 3 THEN 1 ELSE 0 END) AS Field1,
						SUM(CASE WHEN t5.C0 = 4 THEN 1 ELSE 0 END) AS Field1_2,
						SUM(CASE WHEN t5.C0 = 5 THEN 1 ELSE 0 END) AS Field1_5,
						SUM(CASE WHEN t5.C0 = 6 THEN 1 ELSE 0 END) AS Field2,
						SUM(CASE WHEN t5.C0 = 7 THEN 1 ELSE 0 END) AS Field3,
						SUM(CASE WHEN t5.C0 = 8 THEN 1 ELSE 0 END) AS Field4,
						SUM(CASE WHEN t5.C0 = 9 THEN 1 ELSE 0 END) AS Field5,
						SUM(CASE WHEN t5.C0 In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
					FROM CTE c
					Join [Db70].dbo.T5 t5
						On t5.C0=c.WaterDiameterId
                    WHERE 
						c.RN=1 AND
						c.CounterStateCode=1 
					Group By c.ZoneTitle";
        }
    }
}
