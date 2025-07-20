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
        { }

        public async Task<ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationDataOutputDto>> Get(MalfunctionMeterByDurationInputDto input)
        {
            string malfunctionMeterByDurationQueryString = GetMalfunctionMeterByDurationQuery();
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                fromDate=input.FromDateJalali,
                toDate=input.ToDateJalali,
                zoneIds = input.ZoneIds,
                malfunctionPeriodCount=input.MalfunctionPeriodCount
            };
            IEnumerable<MalfunctionMeterByDurationDataOutputDto> malfunctionMeterByDurationData = await _sqlReportConnection.QueryAsync<MalfunctionMeterByDurationDataOutputDto>(malfunctionMeterByDurationQueryString, @params,null, 180);
            MalfunctionMeterByDurationHeaderOutputDto malfunctionMeterByDurationHeader = new MalfunctionMeterByDurationHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (malfunctionMeterByDurationData is not null && malfunctionMeterByDurationData.Any()) ? malfunctionMeterByDurationData.Count() : 0,
            };

            ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationDataOutputDto> result = new ReportOutput<MalfunctionMeterByDurationHeaderOutputDto, MalfunctionMeterByDurationDataOutputDto>(ReportLiterals.MalfunctionMeterByDuration, malfunctionMeterByDurationHeader, malfunctionMeterByDurationData);
            return result;
        }

        private string GetMalfunctionMeterByDurationQuery()
        {
            return @"WITH BillsWithRowNum AS (
                        SELECT 
                            b.BillId,
                            b.CustomerNumber,
                            b.ZoneId,
                    		b.ZoneTitle,
                    		b.ReadingNumber,
                    		b.UsageTitle,
                    		b.ConsumptionAverage,
                            b.CounterStateCode,
                            b.RegisterDay,
                    		b.ContractCapacity,
                            ROW_NUMBER() OVER (PARTITION BY b.BillId ORDER BY b.RegisterDay DESC) AS rn
                        FROM [CustomerWarehouse].dbo.Bills b
                    ),
                    ValidLatestBills AS (
                        SELECT 
                            bwr.BillId,
                    		bwr.ZoneTitle,
                    		bwr.ReadingNumber,
                    		bwr.UsageTitle,
                    		bwr.ConsumptionAverage,
                            bwr.CounterStateCode,
                            bwr.RegisterDay AS LatestRegisterDay,
                            bwr.CustomerNumber,
                            bwr.ZoneId,
                    		bwr.ContractCapacity AS ContractualCapacity
                        FROM BillsWithRowNum bwr
                        WHERE 
                            bwr.rn = 1 AND 
                            bwr.CounterStateCode = 1 AND
                            NOT EXISTS (
                                SELECT 1
                                FROM [CustomerWarehouse].dbo.MeterChange mc
                                WHERE mc.CustomerNumber = bwr.CustomerNumber 
                                  AND mc.ZoneId = bwr.ZoneId
                                  AND mc.ChangeDateJalali > bwr.RegisterDay
                            )
                    ),
                    BillWithGroups AS (
                        SELECT 
                            b.BillId,
                            b.RegisterDay,
                            b.CounterStateCode,
                            v.LatestRegisterDay,
                            SUM(CASE WHEN b.CounterStateCode = 1 THEN 0 ELSE 1 END)
                                OVER (PARTITION BY b.BillId ORDER BY b.RegisterDay DESC) AS group_id
                        FROM [CustomerWarehouse].dbo.Bills b
                        INNER JOIN ValidLatestBills v ON v.BillId = b.BillId
                        WHERE b.RegisterDay <= v.LatestRegisterDay
                    ),
                    FinalCount AS (
                        SELECT 
                            BillId,
                            COUNT(*) AS MalfunctionPeriodCount
                        FROM BillWithGroups
                        WHERE CounterStateCode = 1 AND group_id = 0
                        GROUP BY BillId
                    ),
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
                    		c.PhoneNo AS PhoneNumber,
                    		c.WaterInstallDate AS MeterInstallationDateJalali
                    		--MeterLife in C#
                    	FROM [CustomerWarehouse].dbo.Clients c
                    	Where c.ToDayJalali IS NULL
                    
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
                        ISNULL(LastChange.ChangeDateJalali, 0) AS LastChangeDateJalali,
                    	c.FullName,
                    	c.FirstName,
                    	c.Surname,
                    	c.MeterDiameterTitle,
                    	c.DomesticUnit,
                    	c.CommercialUnit,
                    	c.OtherUnit,
                    	c.Address,
                    	c.PhoneNumber,
                    	c.MeterInstallationDateJalali--
                    FROM ValidLatestBills v
                    JOIN FinalCount f ON v.BillId = f.BillId
                    JOIN ClientData c ON v.BillId = c.BillId
                    OUTER APPLY (
                        SELECT TOP 1 mc.ChangeDateJalali
                        FROM [CustomerWarehouse].dbo.MeterChange mc
                        WHERE mc.CustomerNumber = v.CustomerNumber AND mc.ZoneId = v.ZoneId
                        ORDER BY mc.ChangeDateJalali DESC
                    ) AS LastChange
                    WHERE
                       v.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber AND
                    	f.MalfunctionPeriodCount=@malfunctionPeriodCount AND 
                    	v.ZoneId IN @zoneIds
                    ORDER BY LastChange.ChangeDateJalali DESC";
        }
    }
}
