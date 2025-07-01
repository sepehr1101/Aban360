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

            BranchHistoryInfoDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<BranchHistoryInfoDto>(branchHistoryQuery, new { billId });
			if (result == null)
				throw new InvalidIdException();

			BranchHistoryBillDataOutputDto historyBillData = await _sqlReportConnection.QueryFirstOrDefaultAsync<BranchHistoryBillDataOutputDto>(billsData, new {billId });
			result.LastMeterReadingDate = historyBillData.LastMeterReadingDate;
			result.LastWaterBillRefundDate= historyBillData.LastWaterBillRefundDate;
			result.LastSubscriptionRefundDate= historyBillData.LastSubscriptionRefundDate;
			result.LastTemporaryDisconnectionDate = historyBillData.LastTemporaryDisconnectionDate;

            result.LastPaymentDate = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(lastPaymentQuery, new { billId });
			result.WaterReplacementDate = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(waterReplacementDateQuery, new { zoneId = result.ZoneId, customerNumber = result.CustomerNumber });



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
						c.RegisterDayJalali AS WaterRegistrationDate,
						'' AS GuaranteeDate,
						'' AS LastReconnectionDate,
						'' AS WaterSubscriptionCancellationDate,
						c.RegisterDayJalali AS LattestChangeMianInfoDate,
						'' AS HouseholdCountStartDate,
						'' AS HouseholdCountEndDate,
						c.SewageRequestDate SewageRequestDate,
						c.SewageInstallDate AS SewageInstallationDate,
						'' AS SewageRegistrationDate,
						'' AS SiphonReplacementDate
					from [CustomerWarehouse].dbo.Clients c
					where 
						c.BillId='116416' AND
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

		private string GetDataInBillsQuery()
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


    }
	
}
