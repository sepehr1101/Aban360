using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.Transactions;
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
		public async Task<LatestReadingBillDataOutputDto> GetInfo(ZoneIdAndCustomerNumberOutputDto input)
        {
			string dbName = GetDbName(input.ZoneId);
            string query = GetQueryWithLatestDb(dbName);
            LatestReadingBillDataOutputDto data = await _sqlReportConnection.QueryFirstOrDefaultAsync<LatestReadingBillDataOutputDto>(query,input);

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
		private string GetQueryWithLatestDb(string dbName)
		{
			return @$";WITH Bill As(
						Select 
							id,
							eshtrak,
							today_date,
							today_no,							
							rate,
							date_bed,
							town,
							radif,
							cod_vas,
							mamor,
							operator,
							rn=ROW_NUMBER() OVER(PARTITION BY b.town, b.radif Order By b.date_bed Desc)
						From [{dbName}].dbo.bed_bes b
						Where 
							b.town=@zoneId AND
							b.radif=@customerNumber AND
							b.cod_vas NOT IN (4,7,8)
					)
					Select 
						b.id,
						b.eshtrak ReadingNumber,
						d.id DeletionStateId,
						d.Title DeletionStateTitle,
						b.today_no LatestMeterNumber ,
						b.rate LatestConsumptionAverage ,
						b.cod_vas CounterStateCode ,
						c.Title CounterStateTitle ,
						b.date_bed latestSuccessfullReadingDateJalali,
						IIF(b.mamor=888,N'خوداظهاری غیرحضوری',IIF(b.mamor=999,N'خوداظهاری حضوری',IIF(b.mamor=0,N'بدون کد مامور',N'دارای کد مامور'))) IssueType,
						b.operator ReaderName
					From Bill b
					Left Join [{dbName}].dbo.members m
						On b.town=m.town AND b.radif=m.radif
					Left Join [Db70].dbo.DeletionState d	
						On m.hasf=d.Id
					Left Join [Db70].dbo.CounterVaziat c
						On b.cod_vas=c.MoshtarakinId
					Where b.rn=1";
		}
	}
}
