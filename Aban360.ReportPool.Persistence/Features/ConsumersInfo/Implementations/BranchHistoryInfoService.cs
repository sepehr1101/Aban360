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
            BranchHistoryInfoDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<BranchHistoryInfoDto>(branchHistoryQuery, new { billId });

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
						c.WaterRequestDate , 
						c.WaterInstallDate AS WaterInstallationDate,
						c.RegisterDayJalali AS WaterRegistrationDate,
						m.ChangeDateJalali AS WaterReplacementDate,
						'' AS GuaranteeDate,
						Case	
							When b.BranchType=N'کمیته امداد' Then N'کمیته امداد'	
							When b.BranchType=N'بهزیستی' Then N'بهزیستی'
						End LastTemporaryDisconnectionDate,
						'' AS LastReconnectionDate,
						'' AS WaterSubscriptionCancellationDate,
						b.RegisterDay AS LastMeterReadingDate,
						p.RegisterDay AS LastPaymentDate,
						c.RegisterDayJalali AS LattestChangeMianInfoDate,
						b.TypeId AS LastWaterBillRefundDate,--اخرین برگشتی اب بها
						b.RegisterDay AS LastSubscriptionRefundDate,--حق انشعاب
						'' AS HouseholdCountStartDate,
						'' AS HouseholdCountEndDate,
						c.SewageRequestDate SewageRequestDate,
						c.SewageInstallDate AS SewageInstallationDate,
						'' AS SewageRegistrationDate,
						'' AS SiphonReplacementDate
					from [CustomerWarehouse].dbo.Clients c
					join [CustomerWarehouse].dbo.MeterChange m on c.CustomerNumber=m.CustomerNumber and c.ZoneId=m.ZoneId
					join [CustomerWarehouse].dbo.Bills b on b.BillId=c.BillId
					join [CustomerWarehouse].dbo.Payments p on p.BillTableId=b.Id
					join [CustomerWarehouse].dbo.RequestBillDetails be on c.BillId COLLATE SQL_Latin1_General_CP1_CI_AS = be.BillId COLLATE SQL_Latin1_General_CP1_CI_AS
					where 
						c.BillId=@billId AND
					    c.ToDayJalali is null 
					Order by
						c.RegisterDayJalali Desc,
						m.RegisterDateJalali Desc ,
						b.RegisterDay Desc,
						be.RegisterDay Desc";
		}
    
    }
}
