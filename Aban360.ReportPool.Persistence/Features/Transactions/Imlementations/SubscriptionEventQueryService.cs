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
            IEnumerable<EventsSummaryDto> result = await _sqlConnection.QueryAsync<EventsSummaryDto>(query, new { billId = billId });
            if (result.Any())
            {
                result = result.OrderBy(i => i.RegisterDate);
            }
            return result;
        }
        public async Task<IEnumerable<EventsSummaryDto>> GetBillDto(string billId)
        {
            string query = GetSubscriptionEventsQuery();
            IEnumerable<EventsSummaryDto> result = await _sqlConnection.QueryAsync<EventsSummaryDto>(query, new { billId = billId });
            if (result.Any())
            {
                result = result.OrderBy(i => i.RegisterDate);
            }
            return result;
        }
        public async Task<IEnumerable<EventsSummaryDto>> GetBillDto(int zoneId, string registerDate, string fromReadingNumber, string toReadingNumber)
        {
            string query = GetSubscriptionEventsQuerybyZoneAndRegisterDay();
            IEnumerable<EventsSummaryDto> result = await _sqlConnection.QueryAsync<EventsSummaryDto>(query, new { zoneId, registerDate, fromReadingNumber, toReadingNumber });
            if (result.Any())
            {
                result = result.OrderBy(i => i.RegisterDate);
            }
            return result;
        }
        public async Task<IEnumerable<EventsSummaryDto>> GetBillDto(int zoneId, string fromReadingNumber, string toReadingNumber)
        {
            string query = GetSubscriptionEventsQuerybyZone();
            IEnumerable<EventsSummaryDto> result = await _sqlConnection.QueryAsync<EventsSummaryDto>(query, new { zoneId, fromReadingNumber, toReadingNumber });
            if (result.Any())
            {
                result = result.OrderBy(i => i.RegisterDate);
            }
            return result;
        }
        private string GetSubscriptionEventsQuery()
        {
            string query = @"
            use Aban360
            select
	            TRIM(BillId) BillId ,Id,PreviousNumber PreviousMeterNumber,NextNumber NextMeterNumber, PreviousDay PreviousMeterDate,NextDay CurrentMeterDate,RegisterDay RegisterDate,SumItems DebtAmount,0 OweAmount,TypeId as [Description], ConsumptionAverage, NULL BankTitle
            from [ReportPool].Bills
            where (BillId)=@billId
            union
            select
	            TRIM(BillId) BillId, Id, 0 PreviousMeterNumber,0 NextMeterNumber,NULL PreviousMeterDate,NULL CurrentMeterDate, RegisterDay RegisterDate, 0 DebtAmount, Amount OweAmount, N'پرداخت' [Description], 0, BankName BankTitle
            from [ReportPool].Payments
            where (BillId)=@billId";
            return query;
        }
        private string GetSubscriptionEventsQuerybyZoneAndRegisterDay()
        {
            string query = @"
            use Aban360
            select
	            TRIM(BillId) BillId, Id,PreviousNumber PreviousMeterNumber,NextNumber NextMeterNumber, PreviousDay PreviousMeterDate,NextDay CurrentMeterDate,RegisterDay RegisterDate,SumItems DebtAmount,0 OweAmount,TypeId as [Description], ConsumptionAverage, NULL BankTitle
            from [ReportPool].Bills
            where 
	            ZoneId=@zoneId AND 
	            RegisterDay=@registerDate AND 
	            TRIM(ReadingNumber) BETWEEN @fromReadingNumber AND @toReadingNumber";
            return query;
        }
        private string GetSubscriptionEventsQuerybyZone()
        {
            string query = @"
            use Aban360
            select
	            TRIM(BillId) BillId, Id,PreviousNumber PreviousMeterNumber,NextNumber NextMeterNumber, PreviousDay PreviousMeterDate,NextDay CurrentMeterDate,RegisterDay RegisterDate,SumItems DebtAmount,0 OweAmount,TypeId as [Description], ConsumptionAverage, NULL BankTitle
            from [ReportPool].Bills
            where 
	            ZoneId=@zoneId AND 
	            TRIM(ReadingNumber) BETWEEN @fromReadingNumber AND @toReadingNumber";
            return query;
        }
    }
}
