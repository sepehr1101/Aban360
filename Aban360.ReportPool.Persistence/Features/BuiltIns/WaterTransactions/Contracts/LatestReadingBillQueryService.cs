using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts
{
    internal sealed class LatestReadingBillQueryService : AbstractBaseConnection, ILatestReadingBillQueryService
    {
        public LatestReadingBillQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<LatestReadingBillDataOutputDto> GetInfo(string billId)
        {
            string query = GetQuery();
            LatestReadingBillDataOutputDto data = await _sqlReportConnection.QueryFirstOrDefaultAsync<LatestReadingBillDataOutputDto>(query, new { BillId = billId });

            return data;
        }
        private string GetQuery()
        {
            return @$";With cte as
					(
						Select 
							ZoneId,
							CustomerNumber,
							BillId,
							ReadingNumber,
							CounterStateCode,
							CounterStateTitle,
							NextNumber,
							ConsumptionAverage,
							NextDay,
							rn=ROW_NUMBER() OVER(Partition By BillId Order By RegisterDay Desc)
						From CustomerWarehouse.dbo.Bills
						Where 
							BillId=@BillId AND
							CounterStateCode NOT IN (4,7,8)
					)
					Select 
						b.BillId,
						b.ReadingNumber,
						b.CounterStateCode CounterStateCode,
						b.CounterStateTitle CounterStateTitle,
						b.NextNumber LatestMeterNumber,
						b.ConsumptionAverage LatestConsumptionAverage,
						b.NextDay LatestSuccessfullReadingDateJalali,
						c.DeletionStateId DeletionStateId,
						c.DeletionStateTitle DeletionStateTitle
					From cte b
					Left Join CustomerWarehouse.dbo.Clients c
						On b.ZoneId=c.ZoneId AND b.CustomerNumber=c.CustomerNumber
					where 
						b.rn=1 AND
						c.ToDayJalali IS NULL";
        }
    }
}
