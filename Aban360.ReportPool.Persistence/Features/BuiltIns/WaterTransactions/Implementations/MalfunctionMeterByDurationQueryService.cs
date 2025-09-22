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
    internal sealed class MalfunctionMeterByDurationQueryService : AbstractBaseConnection, IMalfunctionMeterByDurationQueryService
    {
        public MalfunctionMeterByDurationQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationDataOutputDto>> Get(MalfunctionMeterByDurationInputDto input)
        {
            string malfunctionMeterByDurationQueryString = GetMalfunctionMeterByDurationQuery();
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                zoneIds = input.ZoneIds,
                fromMalfunctionPeriodCount=input.FromMalfunctionPeriodCount,
                toMalfunctionPeriodCount=input.ToMalfunctionPeriodCount,
            };
            IEnumerable<MalfunctionMeterByDurationDataOutputDto> malfunctionMeterByDurationData = await _sqlReportConnection.QueryAsync<MalfunctionMeterByDurationDataOutputDto>(malfunctionMeterByDurationQueryString, @params,null, 180);
            MalfunctionMeterByDurationHeaderOutputDto malfunctionMeterByDurationHeader = new MalfunctionMeterByDurationHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (malfunctionMeterByDurationData is not null && malfunctionMeterByDurationData.Any()) ? malfunctionMeterByDurationData.Count() : 0,
            };

            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationDataOutputDto> result = new ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationDataOutputDto>(ReportLiterals.MalfunctionMeterByDurationDetail, malfunctionMeterByDurationHeader, malfunctionMeterByDurationData);
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
                            b.ReadingNumber,
                            b.UsageTitle,
                            b.ConsumptionAverage,
                            b.Consumption,
	                        b.SumItems,
                            b.CounterStateCode,
                            b.RegisterDay AS LatestRegisterDay,
                            b.ContractCapacity AS ContractualCapacity
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
                    ),
                    -- اطلاعات مشتری
                    ClientData AS (
                        SELECT 
                            c.BillId,
                            TRIM(c.FirstName)+' '+TRIM(c.SureName) AS FullName,
                            TRIM(c.FirstName) AS FirstName,
                            TRIM(c.SureName) AS Surname,
                            c.WaterDiameterTitle AS MeterDiameterTitle,
                            c.DomesticCount AS DomesticUnit,
                            c.CommercialCount AS CommercialUnit,
                            c.OtherCount AS OtherUnit,
                            TRIM(c.Address) AS Address,
							c.BranchType,
							c.MainSiphonTitle AS SiphonDiameterTitle,
                            TRIM(c.PhoneNo) AS PhoneNumber,
                            TRIM(c.MobileNo) AS MobileNumber,
                            c.WaterInstallDate AS MeterInstallationDateJalali,
                            c.WaterRequestDate AS MeterRequestDateJalali,
                            c.DeletionStateTitle
                        FROM [CustomerWarehouse].dbo.Clients c
                        WHERE 
                    		c.ToDayJalali IS NULL AND
                            (@fromReadingNumber IS NULL OR
                             @toReadingNumber IS NULL OR  
                            c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber )AND
                            c.ZoneId IN @zoneIds
                    )
                    SELECT 
                        v.BillId,
                        v.CustomerNumber,
                        v.ZoneId,
                        v.ZoneTitle,
                        v.ReadingNumber,
                        v.UsageTitle,
                        v.ConsumptionAverage,
                        v.CounterStateCode,
                        v.ContractualCapacity,
                        v.LatestRegisterDay,
                        f.MalfunctionPeriodCount,
                        ISNULL(lc.ChangeDateJalali, 0) AS LastChangeDateJalali,
                        c.FullName,
                        c.FirstName,
                        c.Surname,
                        c.MeterDiameterTitle,
                        c.DomesticUnit,
                        c.CommercialUnit,
                        c.OtherUnit,
                        c.Address,
                        c.BranchType,
                        c.PhoneNumber,
                        c.MobileNumber,
                        c.SiphonDiameterTitle,
                        c.MeterInstallationDateJalali,
                        c.MeterRequestDateJalali,
                        c.DeletionStateTitle,
                        v.Consumption,
	                    v.SumItems
                    FROM ValidLatestBills v
                    INNER JOIN FinalCount f 
                    	ON v.BillId = f.BillId AND (f.MalfunctionPeriodCount BETWEEN @fromMalfunctionPeriodCount AND @toMalfunctionPeriodCount)
                    INNER JOIN ClientData c 
                    	ON v.BillId = c.BillId
                    OUTER APPLY (
                        SELECT TOP 1 mc.ChangeDateJalali
                        FROM [CustomerWarehouse].dbo.MeterChange mc
                        WHERE 
                    		mc.CustomerNumber = v.CustomerNumber AND 
                            mc.ZoneId = v.ZoneId
                        ORDER BY mc.ChangeDateJalali DESC
                    ) lc
                    ORDER BY lc.ChangeDateJalali DESC;";
        }
    }
}
