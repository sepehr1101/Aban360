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
            string branchHistoryQuery = GetIndividualsSummayDtoQuery();
            BranchHistoryInfoDto result = await _sqlConnection.QueryFirstOrDefaultAsync<BranchHistoryInfoDto>(branchHistoryQuery, new { billId });

            return result;
        }
        private string GetIndividualsSummayDtoQuery()
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
						
						e.ValidFrom as 'ResidenceStartDate',
						N'---' as 'VacancyStartDate',
						N'---' as 'VacancyEndDate',
						
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
    
    }
}
