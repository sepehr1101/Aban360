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
    internal sealed class MalfunctionMeterByDurationSummaryByZoneQueryService : AbstractBaseConnection, IMalfunctionMeterByDurationSummaryByZoneQueryService
    {
        public MalfunctionMeterByDurationSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryByZoneDataOutputDto>> Get(MalfunctionMeterByDurationInputDto input)
        {
            string malfunctionMeterByDurationQueryString = GetMalfunctionMeterByDurationQuery();
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                zoneIds = input.ZoneIds,
                fromMalfunctionPeriodCount = input.FromMalfunctionPeriodCount,
                toMalfunctionPeriodCount = input.ToMalfunctionPeriodCount,
            };
            IEnumerable<MalfunctionMeterByDurationSummaryByZoneDataOutputDto> malfunctionMeterByDurationData = await _sqlReportConnection.QueryAsync<MalfunctionMeterByDurationSummaryByZoneDataOutputDto>(malfunctionMeterByDurationQueryString, @params, null, 180);
            MalfunctionMeterByDurationHeaderOutputDto malfunctionMeterByDurationHeader = new MalfunctionMeterByDurationHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = malfunctionMeterByDurationData is not null && malfunctionMeterByDurationData.Any() ? malfunctionMeterByDurationData.Count() : 0,
            };

            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryByZoneDataOutputDto> result = new ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationSummaryByZoneDataOutputDto>(ReportLiterals.MalfunctionMeterByDurationSummary + ReportLiterals.ByZone, malfunctionMeterByDurationHeader, malfunctionMeterByDurationData);
            return result;
        }

        private string GetMalfunctionMeterByDurationQuery()
        {
            return @"-- آخرین قبض معتبر
                    ;WITH ValidLatestBills AS (
                        SELECT
                            b.BillId,
				    		b.CustomerNumber,
                            b.ZoneId,
                            b.ZoneTitle,
                            b.UsageTitle,
                            b.CounterStateCode,
                            b.RegisterDay AS LatestRegisterDay
                        FROM (
                            SELECT *,
                                   ROW_NUMBER() OVER (PARTITION BY BillId ORDER BY RegisterDay DESC) AS rn
                            FROM [CustomerWarehouse].dbo.Bills
                            WHERE 
                       			ZoneId IN @zoneIds AND 
                       			CounterStateCode NOT IN (4,7,8) 
                        ) b
                        WHERE 
                       		b.rn = 1 AND 
                       		b.CounterStateCode = 1 AND 
                       		NOT EXISTS (--بعد از اخرین قبض تعویض شده اند
                              SELECT 1
                              FROM [CustomerWarehouse].dbo.MeterChange mc
                              WHERE 
                       			mc.CustomerNumber = b.CustomerNumber
                                AND mc.ZoneId = b.ZoneId
                                AND mc.ChangeDateJalali > b.RegisterDay
                          )
                    ),
                    -- محاسبه تعداد دوره‌های خرابی
                    FinalCount AS (
                        SELECT 
                            b.BillId,
                            COUNT(1) AS MalfunctionPeriodCount
                        FROM [CustomerWarehouse].dbo.Bills b
                        INNER JOIN ValidLatestBills v 
                       		ON v.CustomerNumber = b.CustomerNumber AND v.ZoneId=b.ZoneId
                        WHERE 
                       	  b.CounterStateCode = 1 AND 
                       	  b.RegisterDay <= v.LatestRegisterDay
                        GROUP BY b.BillId
                    )
                    SELECT 
				    		MAX(t46.C2) AS RegionTitle,
                            v.ZoneTitle,
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
                    FROM ValidLatestBills v
                    INNER JOIN FinalCount f 
                       	ON v.BillId = f.BillId AND (f.MalfunctionPeriodCount BETWEEN @fromMalfunctionPeriodCount AND @toMalfunctionPeriodCount)
				    INNER JOIN [CustomerWarehouse].dbo.Clients c
				    	On c.BillId=v.BillId
				    Join [Db70].dbo.T51 t51
				    	On t51.C0=c.ZoneId
				    Join [Db70].dbo.T46 t46
				    	On t51.C1=t46.C0
				    Join [Db70].dbo.T5 t5
				    	On t5.C0=c.WaterDiameterId
                    WHERE 
                    		c.ToDayJalali IS NULL AND
                            (@fromReadingNumber IS NULL OR
                             @toReadingNumber IS NULL OR  
                            c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber )AND
                            c.ZoneId IN @zoneIds
				    Group by v.ZoneTitle";
        }
    }
}
