using Aban360.ReportPool.Domain.Features.Dto;
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
        private string GetSubscriptionEventsQuery()
        {
            string query = @"
            use CustomerWarehouse
            select
	            Id,PreviousNumber PreviousMeterNumber,NextNumber NextMeterNumber, PreviousDay PreviousMeterDate,NextDay CurrentMeterDate,RegisterDay RegisterDate,SumItems DebtAmount,0 OweAmount,TypeId as [Description], ConsumptionAverage, NULL BankTitle
            from Bills
            where (BillId)='10018315'
            union
            select
	            Id, 0 PreviousMeterNumber,0 NextMeterNumber,NULL PreviousMeterDate,NULL CurrentMeterDate, RegisterDay RegisterDate, 0 DebtAmount, Amount OweAmount, N'پرداخت' [Description], 0, BankName BankTitle
            from Payments
            where (BillId)=@billId";
            return query;
        }
    }
}
