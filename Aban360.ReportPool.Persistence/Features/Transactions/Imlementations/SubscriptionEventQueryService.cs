using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Transactions.Imlementations
{
    internal sealed class SubscriptionEventQueryService : AbstractBaseConnection, ISubscriptionEventQueryService
    {
        public SubscriptionEventQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IEnumerable<EventsSummaryDto>> GetEventsSummaryDtos(string billId)
        {
            string query = GetSubscriptionEventsQuery();
            IEnumerable<EventsSummaryDto> result = await _sqlReportConnection.QueryAsync<EventsSummaryDto>(query, new { billId = billId });
            if (result.Any())
            {
                result = result.OrderBy(i => i.RegisterDate);
            }
            return result;
        }
        public async Task<IEnumerable<EventsSummaryDto>> GetBillDto(string billId)
        {
            string query = GetSubscriptionEventsQuery();
            IEnumerable<EventsSummaryDto> result = await _sqlReportConnection.QueryAsync<EventsSummaryDto>(query, new { billId = billId });
            if (result.Any())
            {
                result = result.OrderBy(i => i.RegisterDate);
            }
            return result;
        }
        public async Task<IEnumerable<EventsSummaryDto>> GetBillDto(int zoneId, string registerDate, string fromReadingNumber, string toReadingNumber)
        {
            string query = GetSubscriptionEventsQuerybyZoneAndRegisterDay();
            IEnumerable<EventsSummaryDto> result = await _sqlReportConnection.QueryAsync<EventsSummaryDto>(query, new { zoneId, registerDate, fromReadingNumber, toReadingNumber });
            if (result.Any())
            {
                result = result.OrderBy(i => i.RegisterDate);
            }
            return result;
        }
        public async Task<IEnumerable<EventsSummaryDto>> GetBillDto(int zoneId, string fromReadingNumber, string toReadingNumber)
        {
            string query = GetSubscriptionEventsQuerybyZone();
            IEnumerable<EventsSummaryDto> result = await _sqlReportConnection.QueryAsync<EventsSummaryDto>(query, new { zoneId, fromReadingNumber, toReadingNumber });
            if (result.Any())
            {
                result = result.OrderBy(i => i.RegisterDate);
            }
            return result;
        }
        private string GetSubscriptionEventsQuery()
        {
            string query = @"
            use CustomerWarehouse
             select
                 TRIM(BillId) BillId ,
	             Id,
	             PreviousNumber PreviousMeterNumber,
	             NextNumber NextMeterNumber, 
	             PreviousDay PreviousMeterDate,
	             NextDay CurrentMeterDate,
	             RegisterDay RegisterDate,
	             SumItems DebtAmount,
	             0 CreditAmount,
	             TypeId as [Description],
	             ConsumptionAverage, 
	             NULL BankTitle,
	             CommercialCount CommercialUnit,
	             DomesticCount DomesticUnit,
	             OtherCount OtherUnit,
	             EmptyCount EmptyUnit,
	             0 HouseholderNumber,
	             ContractCapacity,
	             UsageId UsageSellId,
	             UsageId2 UsageConsumptionId,
	             UsageTitle UsageSellTitle,
	             UsageTitle2 UsageConsumptionTitle
             from [CustomerWarehouse].dbo.Bills
             where (BillId)=@billId
             union
             select
                 TRIM(BillId) BillId,
	             Id,
	             0 PreviousMeterNumber,
	             0 NextMeterNumber,
	             NULL PreviousMeterDate,
	             NULL CurrentMeterDate,
	             RegisterDay RegisterDate,
	             0 DebtAmount, 
	             Amount CreditAmount,
	             N'پرداخت' [Description],
	             0 ConsumptionAverage,
	             BankName BankTitle,
	             0 CommercialUnit,
	             0 DomesticUnit,
	             0 OtherUnit,
	             0 EmptyUnit,
	             0 HouseholderNumber,
	             0 ContractCapacity,
	             0 UsageSellId,
	             0 UsageConsumptionId,
	             '' UsageSellTitle,
	             '' UsageConsumptionTitle
             from [CustomerWarehouse].dbo.Payments
             where (BillId)=@billId";
            return query;
        }
        private string GetSubscriptionEventsQuerybyZoneAndRegisterDay()
        {
            string query = @"
            use CustomerWarehouse
            select
	            TRIM(BillId) BillId, Id,PreviousNumber PreviousMeterNumber,NextNumber NextMeterNumber, PreviousDay PreviousMeterDate,NextDay CurrentMeterDate,RegisterDay RegisterDate,SumItems DebtAmount,0 CreditAmount,TypeId as [Description], ConsumptionAverage, NULL BankTitle
            from [CustomerWarehouse].dbo.Bills
            where 
	            ZoneId=@zoneId AND 
	            RegisterDay=@registerDate AND 
	            TRIM(ReadingNumber) BETWEEN @fromReadingNumber AND @toReadingNumber";
            return query;
        }

        public async Task<IEnumerable<BranchEventsDto>> GetBranchEventDtos(string billId)
        {
            string query = GetBranchEventsSummaryQuery();
            IEnumerable<BranchEventsDto> result = await _sqlReportConnection.QueryAsync<BranchEventsDto>(query, new { billId = billId });
            if (result.Any())
            {
                result = result.OrderBy(i => i.RegisterDate);
            }
            return result;

        }
        private string GetBranchEventsSummaryQuery()
        {
            string query =
                @"USE [CustomerWarehouse]
                SELECT 
                    N'صدور صورتحساب' [Description],
                    TrackNumber, 
                    RegisterDay RegisterDate,
                    AmountSum DebtAmount ,
                    0 CreditAmount 
                from [TerminatedRequestsV2]
                WHERE TRIM(BillId)=@billId
                UNION
                SELECT 
                    N'پرداخت'+ N'('+ BankName+' '+PaymentGateway+N')' [Description],
                    '' TrackNumber,
                    RegisterDay RegisterDate,
                    0 DebtAmount, 
                    Amount CreditAmount
                FROM [PaymentsEn]
                WHERE TRIM(BillId)=@billId";
            return query;
        }
        private string GetSubscriptionEventsQuerybyZone()
        {
            string query = @"
            use CustomerWarehouse
            select
	            TRIM(BillId) BillId, Id,PreviousNumber PreviousMeterNumber,NextNumber NextMeterNumber, PreviousDay PreviousMeterDate,NextDay CurrentMeterDate,RegisterDay RegisterDate,SumItems DebtAmount,0 OweAmount,TypeId as [Description], ConsumptionAverage, NULL BankTitle
            from [CustomerWarehouse].dbo.Bills
            where 
	            ZoneId=@zoneId AND 
	            TRIM(ReadingNumber) BETWEEN @fromReadingNumber AND @toReadingNumber";
            return query;
        }
    }
}
