using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal class LatestWaterMeterInfoQueryService : AbstractBaseConnection, ILatestWaterMeterInfoQueryService
    {
        public LatestWaterMeterInfoQueryService(IConfiguration configuration)
            : base(configuration)
        { }
        public async Task<LatestWaterMeterInfoDto> GetInfo(string billId)
        {
            string latestInfoQuery = GetLatestInfoQuery();
            string latestMeterChangeDateQuery = GetMeterChangeNumberQuery();
            string latestDisconnectionBranchDateQuery = GetLatestDisconnectionBranchQuery();
            double pattern = 12.5;

            LatestWaterMeterInfoDto latestData = await _sqlReportConnection.QueryFirstAsync<LatestWaterMeterInfoDto>(latestInfoQuery, new { billId = billId });
            latestData.LatestMainChangeDate = await _sqlReportConnection.QueryFirstAsync<string>(latestMeterChangeDateQuery, new { billId = billId, customerNumber = latestData.CustomerNumber, zoneId = latestData.ZoneId });
            latestData.LatestTemporarilyDisconnectionBranch = await _sqlReportConnection.QueryFirstAsync<string>(latestDisconnectionBranchDateQuery, new { billId = billId });
            latestData.ConsumptionState = CalcConsumptionState(latestData.ConsumptionAverage, latestData.ContractualCapacity == 0 ? pattern : latestData.ContractualCapacity);
           
			return latestData;
        }
        private string GetLatestInfoQuery()
        {
            return @"Select top 1
						c.ZoneId AS ZoneId,
						c.CustomerNumber AS CustomerNumber,
						b.ConsumptionAverage AS ConsumptionAverage,
						b.ContractCapacity AS ContractualCapacity,
						b.ZoneId,
						w.Debt AS WaterDebt,
						v.BedehiAll AS BranchDebt,
						p.RegisterDay AS LatestWaterPaid,
						' ' AS ConsumptionState,--todo
						b.CounterStateTitle AS MeterStatusTitle,
						b.NextNumber AS LatestMeterNumber,
						0 AS MeterLife,
						' ' AS MeterReplacementDate,--complete in other query string
						b.NextDay AS LatestMeterReading,
						c.BranchType AS UseStateTitle,
						0 AS PossibilityEmptyUnit,
						' ' AS LatestTemporarilyDisconnectionBranch,--todo
						c.DeletionStateTitle AS BranchStatus,
						c.HasCommonSiphon AS CommonSiphon,
						0 AS TagStatus,
						0 AS IsContaminated,
						c.RegisterDayJalali AS LatestMainChangeDate
					From [CustomerWarehouse].dbo.Clients c
					Join [CustomerWarehouse].dbo.Bills b on c.BillId=b.BillId
					Join [CustomerWarehouse].dbo.Payments p on p.BillTableId=b.Id
					Join [CustomerWarehouse].dbo.WaterDebt w on c.BillId=w.BillId
					Join [CustomerWarehouse].dbo.VosoolEnsheabAlert v on c.BillId=b.BillId
					Where
						c.BillId=@billId AND
						b.TypeId=N'قبض'
					Order by 
						c.RegisterDayJalali Desc,
						b.RegisterDay Desc,
						p.RegisterDay Desc";
        }

        private string GetMeterChangeNumberQuery()
        {
            return @"Select	
						m.ChangeDateJalali
					From [CustomerWarehouse].dbo.MeterChange m
					where	
						m.ZoneId=@zoneId AND
						m.CustomerNumber=@customerNumber
					Order By
						m.RegisterDateJalali";

        }

        private string GetLatestDisconnectionBranchQuery()
        {
            return @"Select top 1
						c.RegisterDayJalali
					From [CustomerWarehouse].dbo.Clients c
					Where 
						c.BillId='52216419' AND
						c.DeletionStateTitle=N'حذف موقت'";

        }

        private string CalcConsumptionState(float consumption, double pattern)
        {
            List<string> data = new List<string> { "خوش مصرف", "پر مصرف", "بد مصرف" };

            if (consumption < pattern)
                return data[0];
            else if (consumption <= 2 * pattern)
                return data[1];
            else
                return data[2];
        }
    }
}