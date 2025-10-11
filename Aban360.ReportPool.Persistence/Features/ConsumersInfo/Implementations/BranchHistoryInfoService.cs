using Aban360.Common.Db.Exceptions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class BranchHistoryInfoService : AbstractBaseConnection, IBranchHistoryInfoService
    {
        public BranchHistoryInfoService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<BranchHistoryInfoDto> GetInfo(string billId)
        {
            //string branchHistoryQuery = GetBranchHistorySummaryDtoQuery();
            string branchHistoryQuery = GetBranchHistorySummaryDtoWithClientDbQuery();
            string lastPaymentQuery = GetLastPaymentDateQuery();
            string waterReplacementDateQuery = GetWaterReplacementDateQuery();
            string billsData = GetDataInBillsQuery();
            string latestHouseholdDateQuery = GetLatestHouseholdDateQuery();
            string latestTemporarilyDeletionQuery = GetLatestTemporarilyDeletionDateQuery();
            string latestReconnectionQuery = GetLatestReconnectionDateQuery();
			string latestCancellationQuery = GetLatestCancellationDateQuery();

            BranchHistoryInfoDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<BranchHistoryInfoDto>(branchHistoryQuery, new { billId });
            if (result == null)
                throw new InvalidIdException();

            BranchHistoryBillDataOutputDto historyBillData = await _sqlReportConnection.QueryFirstOrDefaultAsync<BranchHistoryBillDataOutputDto>(billsData, new { billId });
            if (historyBillData == null)
                throw new InvalidIdException();

            result.LastMeterReadingDate = historyBillData.LastMeterReadingDate;
           // result.LastWaterBillRefundDate = historyBillData.LastWaterBillRefundDate;
            result.LastWaterBillRefundDate = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(GetLatestWaterRefundDateQuery(), new { billId });
			// result.LastSubscriptionRefundDate = historyBillData.LastSubscriptionRefundDate;
			result.LastSubscriptionRefundDate = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(GetLatestServiceLinkRefundDateQuery(), new { billId });
            result.LastTemporaryDisconnectionDate = historyBillData.LastTemporaryDisconnectionDate;

            result.LastPaymentDate = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(lastPaymentQuery, new { billId });
            result.WaterReplacementDate = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(waterReplacementDateQuery, new { zoneId = result.ZoneId, customerNumber = result.CustomerNumber });
            result.HouseholdCountStartDate = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(latestHouseholdDateQuery, new { billId });
            result.LastTemporaryDisconnectionDate = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(latestTemporarilyDeletionQuery, new { billId });
            result.LastReconnectionDate = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(latestReconnectionQuery, new { billId = billId, latestDeletionState = result.LastTemporaryDisconnectionDate });
			result.WaterSubscriptionCancellationDate = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(latestCancellationQuery, new { billId });
			
			return result;
        }
        private string GetBranchHistorySummaryDtoQuery()
        {
            return @"select top 1
						N'---' as 'WaterRequestDate',
						w.InstallationDate as 'WaterInstallationDate',
						w.ProductDate as 'WaterRegistrationDate',
					
						N'---' as 'WaterReplacementDate',
						w.GuaranteeDate as 'GuaranteeDate',
						N'---' as 'LastTemporaryDisconnectionDate',
						
						N'---' as 'LastReconnectionDate',
						N'---' as 'WaterSubscriptionCancellationDate',
						N'---' as 'LastMeterReadingDate',
					
					    c.ValidFrom as 'LastPaymentDate',
						N'---' as 'LattestChangeMianInfoDate',
						N'---' as 'LastWaterBillRefundDate',
						
						N'---' as 'LastSubscriptionRefundDate',
						N'---' as 'HouseholdCountStartDate',
						N'---' as 'HouseholdCountEndDate',
						
						s.ValidFrom as 'SewageRequestDate', 
						s.InstallationDate as 'SewageInstallationDate',
						s.InstallationDate as 'SewageRegistrationDate',
						
						N'---' as 'SiphonReplacementDate'
					from ClaimPool.WaterMeter w
						join ClaimPool.WaterMeterSiphon ws on w.Id=ws.WaterMeterId
						join ClaimPool.Siphon s on ws.SiphonId=s.Id
						join PaymentPool.Credit c on w.BillId=c.BillId
						join ClaimPool.Estate e on w.EstateId=e.Id
					where w.BillId=@billId
					order by c.ValidFrom desc";

        }

        private string GetBranchHistorySummaryDtoWithClientDbQuery()
        {
            return @"select Top 1	
						c.ZoneId AS ZoneId,
						c.CustomerNumber AS CustomerNumber,
						c.WaterRequestDate , 
						c.WaterInstallDate AS WaterInstallationDate,
						c.WaterRegisterDateJalali AS WaterRegistrationDate,
						'' AS GuaranteeDate,
						'' AS LastReconnectionDate,
						'' AS WaterSubscriptionCancellationDate,
						c.RegisterDayJalali AS LattestChangeMianInfoDate,
						'' AS HouseholdCountStartDate,
						'' AS HouseholdCountEndDate,
						c.SewageRequestDate AS SewageRequestDate,
						c.SewageInstallDate AS SewageInstallationDate,
						c.SewageRegisterDateJalali AS SewageRegistrationDate,
						'' AS SiphonReplacementDate
					from [CustomerWarehouse].dbo.Clients c
					where 
						c.BillId=@billId AND
					    c.ToDayJalali is null 
					Order by
						c.RegisterDayJalali Desc";
        }

        private string GetLastPaymentDateQuery()
        {
            return @"Select Top 1 p.RegisterDay
					 From [CustomerWarehouse].dbo.Payments p
					 Where p.BillId=@billId
					 Order By p.RegisterDay Desc";

        }

        private string GetWaterReplacementDateQuery()
        {
            return @"Select m.ChangeDateJalali
					From [CustomerWarehouse].dbo.MeterChange m
					Where 
						m.ZoneId=@zoneId AND
						m.CustomerNumber=@customerNumber
					Order By 
						m.RegisterDateJalali Desc";

        }

        private string GetDataInBillsQuery()//todo:check
        {
				return @"Select Top 1
							b.RegisterDay AS LastMeterReadingDate,
							b.TypeId AS LastWaterBillRefundDate,--اخرین برگشتی اب بها
							b.RegisterDay AS LastSubscriptionRefundDate,--حق انشعاب
							Case	
								When b.BranchType=N'کمیته امداد' Then N'کمیته امداد'	
								When b.BranchType=N'بهزیستی' Then N'بهزیستی'
							End LastTemporaryDisconnectionDate
						From [CustomerWarehouse].dbo.Bills b
						Where b.BillId=@billId
						Order By b.RegisterDay Desc";
        }

		private string GetLatestWaterRefundDateQuery()
		{
			return @"Select Top 1
						b.RegisterDay AS LastWaterBillRefundDate--اخرین برگشتی اب بها
					From [CustomerWarehouse].dbo.Bills b
					Where 
						b.BillId=@billId AND 
						b.TypeCode = 5 
					Order By 
						b.RegisterDay Desc";
		}
		
		private string GetLatestServiceLinkRefundDateQuery()
		{
			return @"Select Top 1		
						r.RegisterDate AS LastSubscriptionRefundDate--حق انشعاب
					From [CustomerWarehouse].dbo.RequestBillDetails r
					Where 
						r.BillId=@billId AND 
						r.TypeCode = 4
					Order By r.RegisterDate Desc";
		}

        private string GetLatestHouseholdDateQuery()
        {
            return @"Select top 1 c.HouseholdDateJalali
					From [CustomerWarehouse].dbo.Clients c
					Where 
						c.BillId=@billId AND
						c.FamilyCount>0 AND
						c.HouseholdDateJalali > '0001/01/01'
					Order By c.HouseholdDateJalali Desc";
        }

        private string GetLatestTemporarilyDeletionDateQuery()
        {
            return @"Select Top 1 c.RegisterDayJalali
					From [CustomerWarehouse].dbo.Clients c
					Where 
						c.BillId=@billId AND
						c.DeletionStateTitle=N'حذف موقت' AND
						c.RegisterDayJalali > '0001/01/01'
					Order by c.RegisterDayJalali Desc";
        }

        private string GetLatestReconnectionDateQuery()
        {
            return @"Select top 1 c.RegisterDayJalali
					From [CustomerWarehouse].dbo.Clients c
					Where	
						c.BillId=@billId AND
						c.RegisterDayJalali>@latestDeletionState AND
						c.DeletionStateTitle=N'انشعاب برقرار'
					Order By c.RegisterDayJalali asc";

        }

		private string GetLatestCancellationDateQuery()
		{
			return @"Select top 1 c.RegisterDayJalali
					From [CustomerWarehouse].dbo.Clients c
					Where	
						c.BillId=@billId AND
						c.DeletionStateTitle=N'جمع آوری'
					Order By c.RegisterDayJalali desc";
		}
    }
}
