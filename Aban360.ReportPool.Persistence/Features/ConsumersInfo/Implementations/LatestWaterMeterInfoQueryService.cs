using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Exceptions;
using Aban360.Common.Literals;
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
            string latestReplacementMeterDateQuery = GetReplacementMeterDateQuery();
            string latestDisconnectionBranchDateQuery = GetLatestDisconnectionBranchQuery();
			string waterDebtQuery = GetWaterDebtQuery();
			string latestPaidQuery = GetLatestPaidQuery();
            string branchDebtQuery= GetBranchDebtQuery();
            string billsDataQuery = GetBillsDataQuery();
            double pattern = 12.5;

            LatestWaterMeterInfoDto latestData = await _sqlReportConnection.QueryFirstOrDefaultAsync<LatestWaterMeterInfoDto>(latestInfoQuery, new { billId = billId });
            if (latestData == null)
            {
                throw new InvalidIdException();
            }
            
            
            LatestWaterMeterBillDataOutputDto LatestBillData = await _sqlReportConnection.QueryFirstOrDefaultAsync<LatestWaterMeterBillDataOutputDto>(billsDataQuery, new { billId = billId });
            latestData.ConsumptionAverage = LatestBillData.ConsumptionAverage;
            latestData.MeterStateTitle= LatestBillData.MeterStatusTitle;
            latestData.LatestMeterNumber = LatestBillData.LatestMeterNumber;
            latestData.LatestMeterReading=LatestBillData.LatestMeterReading;
            
            latestData.WaterDebt = await _sqlReportConnection.QueryFirstOrDefaultAsync<long>(waterDebtQuery, new { billId=billId });
			latestData.LatestWaterPaid=await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(latestPaidQuery,new { billId=billId });
			latestData.BranchDebt=await _sqlReportConnection.QueryFirstOrDefaultAsync<long>(branchDebtQuery,new {billId=billId});
            latestData.MeterReplacementDate= await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(latestMeterChangeDateQuery, new { billId = billId, customerNumber = latestData.CustomerNumber, zoneId = latestData.ZoneId });
            latestData.LatestMainChangeDate= await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(latestReplacementMeterDateQuery, new { billId = billId, customerNumber = latestData.CustomerNumber, zoneId = latestData.ZoneId });
            latestData.LatestTemporarilyDisconnectionBranch = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(latestDisconnectionBranchDateQuery, new { billId = billId });
            latestData.ConsumptionState = CalcConsumptionState(latestData.ConsumptionAverage, latestData.ContractualCapacity == 0 ? pattern : latestData.ContractualCapacity);
           
			return latestData;
        }
        private string GetLatestInfoQuery()
        {
            return @"Select top 1
						c.ZoneId AS ZoneId,
						c.CustomerNumber AS CustomerNumber,
						c.ContractCapacity AS ContractualCapacity,						
						' ' AS ConsumptionState,--todo
						0 AS MeterLife,
						' ' AS MeterReplacementDate,--complete in other query string
						c.BranchType AS UseStateTitle,
						0 AS PossibilityEmptyUnit,
						' ' AS LatestTemporarilyDisconnectionBranch,--todo
						c.DeletionStateTitle AS BranchStatus,
						c.HasCommonSiphon AS CommonSiphon,
						0 AS TagStatus,
						0 AS IsContaminated,
						c.RegisterDayJalali AS LatestMainChangeDate,
                        c.WaterInstallDate as WaterInstallationDateJalali,
						c.DomesticCount as DomesticUnit,
                        c.UsageId
					From [CustomerWarehouse].dbo.Clients c
					Where
						c.BillId=@billId AND
                        c.ToDayJalali IS NULL
					Order by 
						c.RegisterDayJalali Desc";
        }
		private string GetWaterDebtQuery()
		{
			return @"Select Top 1 w.Debt
					 From [CustomerWarehouse].dbo.WaterDebt w
					 Where w.BillId=@billId";

        }
		private string GetLatestPaidQuery()
		{
			return @"Select Top 1 p.RegisterDay
				     From [CustomerWarehouse].dbo.Payments p
				     Where p.BillId=@billId
				     Order By p.RegisterDay Desc";

        }
        private string GetBranchDebtQuery()
        {
            return @"Select v.BedehiAll
                     From [CustomerWarehouse].dbo.VosoolEnsheabAlert v
                     Where v.BillId=@billId";
        }
        private string GetBillsDataQuery()
        {
            return @"Select top 1
                    	b.ConsumptionAverage AS ConsumptionAverage,
                    	b.CounterStateTitle AS MeterStatusTitle,
                    	b.NextNumber AS LatestMeterNumber,
                    	b.NextDay AS LatestMeterReading
                    From [CustomerWarehouse].dbo.Bills b
                    Where 
                    	b.BillId=@billId AND
                    	b.TypeId=N'قبض'
                    Order By b.RegisterDay Desc";
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
						m.RegisterDateJalali Desc";

        }
        
        private string GetReplacementMeterDateQuery()
        {
            return @"Select c.RegisterDayJalali as LatestMainChangeDate
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.BillId=@billId AND
                    	c.ToDayJalali IS NULL";

        }

        private string GetLatestDisconnectionBranchQuery()
        {
            return @"Select top 1
						c.RegisterDayJalali
					From [CustomerWarehouse].dbo.Clients c
					Where 
						c.BillId=@billId AND
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