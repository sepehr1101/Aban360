using Aban360.Common.Db.Exceptions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class BranchSpecificationInfoService : AbstractBaseConnection, IBranchSpecificationInfoService
    {
        public BranchSpecificationInfoService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<BranchSpecificationInfoDto> GetInfo(string billId)
        {
            //string BranchSpecificationQuery = GetBranchSpecificationSummayDtoQuery();
            string BranchSpecificationQuery = GetBranchSpecificationSummaryDtoWithClientDbQuery();
            string meterChangeDateQuery = GetMeterChangeDateQuery();
            string lastChangelSiphonDateQuery = GetLastChangeSiphonDateQuery();

            BranchSpecificationInfoDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<BranchSpecificationInfoDto>(BranchSpecificationQuery, new { billId });
            if (result == null)
                throw new InvalidIdException();

            string latestMeterChangeDate = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(meterChangeDateQuery, new { zoneId = result.ZoneId, customerNumber = result.CustomerNumber });
            result.LatestMeterChangeDate = latestMeterChangeDate;

            result.LastChangeSiphonDate = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(lastChangelSiphonDateQuery, new { billId });
            result.MeterStatusTitle = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(GetBranchStatusQuery(), new { billId = billId });
            result.SiphonsDiameterCount = await _sqlReportConnection.QueryAsync<SiphonsDiameterCount>(GetSiphonDiameterCountWithClientDbQuery(), new { billId });
            return result;
        }
        private string GetBranchSpecificationSummayDtoQuery()
        {
            return @"select 
			    		--water
                    	md.Title as 'MeterDiameterTitle',
			    		w.BodySerial,
                    	w.SealNumber,
                    	mt.Title as 'MeterTypeTitle',
                    	mp.Title as 'MeterProducerTitle',
			    		'---' as 'MeterEquipmentBrokerTitle',
			    		'---' as 'MeterInstallationBrokerTitle',
                    	mi.Title as 'WaterMeterInstallationMethodTitle',
			    		6 as 'MeterLife',
			    		N'سالم' as 'MeterStatusTitle',
			    		'---' as 'WitnessMeter',
			    		--siphon
			    		'---' as 'CommonSiphon',
                    	COUNT(s.InstallationDate) as N'SiphonCount',
                    	sm.Title as 'SiphonMaterialTitle',
			    		6 as 'SiphonLife',
			    		N'---' as 'SiphonInstallationContractor',
			    		N'---' as 'SiphonEquipmentBrokerTitle',
			    		N'---' as 'SiphonInstallationBrokerTitle',
			    		0 as 'LoadOfContamination',
						s.InstallationDate as 'SiphonInstallationDate'
                    from ClaimPool.WaterMeter w
                    join ClaimPool.WaterMeterSiphon ws on ws.WaterMeterId=w.Id
                    join ClaimPool.Siphon s on ws.SiphonId=s.Id
                    join ClaimPool.MeterProducer mp on w.MeterProducerId=mp.Id
                    join ClaimPool.MeterType mt on w.MeterTypeId=mt.Id
                    join ClaimPool.MeterDiameter md on w.MeterDiameterId=md.Id
                    join ClaimPool.WaterMeterInstallationMethod mi on w.WaterMeterInstallationMethodId=mi.Id
                    join ClaimPool.SiphonMaterial sm on s.SiphonMaterialId=sm.Id
                    where w.BillId=@billId
                    group by
                        w.BodySerial,
                    	w.SealNumber,
                    	mp.Title ,
                    	md.Title ,
                    	mt.Title ,
                    	mi.Title,
                    	sm.Title,
                        s.InstallationDate";

        }
        private string GetSiphonDiameterCount()
        {
            return @"SELECT
                    sd.Title AS SiphonDiameterTitle,
                    ISNULL(CountedSiphons.Count, 0) AS SiphonCount
                FROM
                    ClaimPool.SiphonDiameter sd
                LEFT JOIN (
                    SELECT    s.SiphonDiameterId,COUNT(s.Id) AS Count
                    FROM ClaimPool.WaterMeter w
                	JOIN ClaimPool.WaterMeterSiphon ws ON ws.WaterMeterId = w.Id
                    JOIN ClaimPool.Siphon s ON s.Id = ws.SiphonId
                    WHERE
                        w.BillId = @billId
                    GROUP BY
                        s.SiphonDiameterId
                ) AS CountedSiphons ON sd.Id = CountedSiphons.SiphonDiameterId
                ORDER BY
                    sd.Title;";
        }


        private string GetSiphonDiameterCountWithClientDbQuery()
        {
            return @"SELECT 
                        SiphonDiameterTitle,
                        SiphonCount
                    FROM
                    (
                        SELECT 
                             Siphon200, Siphon150, Siphon100, Siphon125, Siphon8, Siphon7, Siphon6, Siphon5
                        FROM [CustomerWarehouse].dbo.Clients
                        WHERE BillId = @billId AND
                        ToDayJalali IS NULL
                    ) src
                    UNPIVOT
                    (
                        SiphonCount FOR SiphonDiameterTitle IN (Siphon200, Siphon150, Siphon100, Siphon125, Siphon8, Siphon7, Siphon6, Siphon5)
                    ) unpvt";
        }

        private string GetBranchSpecificationSummaryDtoWithClientDbQuery()
        {
            return @"select Top 1
                        c.CustomerNumber,
                        c.ZoneId,
                        c.WaterInstallDate AS WaterInstallDate,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	c.MeterSerialBody AS BodySerial,
                    	'' AS SealNumber,
                    	c.BranchType AS MeterTypeTitle,
                    	'' AS MeterProducerTitle,
                    	'' AS MeterEquipmentBrokerTitle,
                    	'' AS MeterInstallationBrokerTitle,
                    	'' AS WaterMeterInstallationMethodTitle,
                    	'0' AS MeterLife,
                    	'' AS MeterStatusTitle,
                    	'' AS WitnessMeter,
                    	c.HasCommonSiphon AS CommonSiphon,
                    	c.Siphon200+c.Siphon150+c.Siphon100+c.Siphon125+c.Siphon8+c.Siphon7+c.Siphon6+c.Siphon5 AS SiphonCount,
                    	'' AS SiphonMaterialTitle,
                    	'0' AS SiphonLife,
                    	'' AS SiphonInstallationContractor,
                    	'' AS SiphonEquipmentBrokerTitle,
                    	'' AS SiphonInstallationBrokerTitle,
						c.SewageInstallDate AS SiphonInstallationDate,
                        c.WaterInstallDate AS WaterInstallDate,
                    	0 AS LoadOfContamination,
                        c.HasSewage AS HasSewage
                    from [CustomerWarehouse].dbo.Clients c
                    where 
						c.BillId=@billId
					    and c.ToDayJalali is null
					Order By
						c.RegisterDayJalali Desc";
        }
        private string GetBranchStatusQuery()
        {
            return @"select top 1 
                    	b.CounterStateTitle AS MeterStatusTitle
                    from [CustomerWarehouse].dbo.Bills b
                    where
                    	b.BillId=@billId AND
                    	b.TypeId=N'قبض'
                    Order by b.RegisterDay desc";
        }
        private string GetMeterChangeDateQuery()
        {
            return @"Select Top 1 mc.ChangeDateJalali
                     From [CustomerWarehouse].dbo.MeterChange mc
                     Where 
                     	mc.ZoneId=@zoneId AND
                     	mc.CustomerNumber=@customerNumber
                     Order by mc.ChangeDateJalali Desc";
        }
        private string GetLastChangeSiphonDateQuery()
        {
            return @"Select Top 1 r.RegisterDate
                    From [CustomerWarehouse].dbo.RequestBillDetails r
                    Where 
                     	r.BillId=@billId AND
                    	r.TypeId=2--todo:
                    Order By r.RegisterDate Desc";
        }
      
    }
}
