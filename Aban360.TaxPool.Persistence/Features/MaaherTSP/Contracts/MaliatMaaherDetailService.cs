using Aban360.Common.Db.Dapper;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.TaxPool.Persistence.Features.MaaherTSP.Contracts
{
    internal sealed class MaliatMaaherDetailService : AbstractBaseConnection, IMaliatMaaherDetailService
    {
        public MaliatMaaherDetailService(IConfiguration configuraion)
            : base(configuraion)
        {
        }

        public async Task Inserts(IEnumerable<MaliatMaaherDetailGetDto> items)
        {
            DataTable table = CreateDataTable(items);

            using (SqlConnection connection = _sqlConnection)
            {
                await connection.OpenAsync();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "Aban360.TaxPool.MaliatMaaherDetail";

                    await bulkCopy.WriteToServerAsync(table);
                }
            }
        }

        public async Task<IEnumerable<MaliatMaaherDetailGetDto>> Get(MaliatMaaherDetailInsertBatchDto input)
        {
            string query = GetTaxQuery();
            IEnumerable<MaliatMaaherDetailGetDto> maaherDetail = await _sqlReportConnection.QueryAsync<MaliatMaaherDetailGetDto>(query, input);
			return maaherDetail;
        }
        public async Task<MaliatMaaherDetailAmountAndCountDto> Get(int wrapperId)
        {
            string query = GetAmountAndCountQuery();
            MaliatMaaherDetailAmountAndCountDto result = await _sqlConnection.QueryFirstOrDefaultAsync<MaliatMaaherDetailAmountAndCountDto>(query, new { wrapperId = wrapperId });

            return result;
        }

        private DataTable CreateDataTable(IEnumerable<MaliatMaaherDetailGetDto> items)
        {
            DataTable table = new DataTable();

            table.Columns.Add("WrapperId", typeof(int));
            table.Columns.Add("indatim", typeof(long));
            table.Columns.Add("inty", typeof(string));
            table.Columns.Add("inno", typeof(long));
            table.Columns.Add("inp", typeof(string));
            table.Columns.Add("ins", typeof(string));
            table.Columns.Add("tob", typeof(string));
            table.Columns.Add("bid", typeof(string));
            table.Columns.Add("bpc", typeof(string));
            table.Columns.Add("radif", typeof(int));
            table.Columns.Add("billId", typeof(string));
            table.Columns.Add("sstid", typeof(string));
            table.Columns.Add("sstt", typeof(string));
            table.Columns.Add("mu", typeof(string));
            table.Columns.Add("am", typeof(int));
            table.Columns.Add("fee", typeof(long));
            table.Columns.Add("dis", typeof(int));
            table.Columns.Add("town", typeof(int));
            table.Columns.Add("date_bed", typeof(string));
            table.Columns.Add("item1", typeof(long));
            table.Columns.Add("item2", typeof(long));
            table.Columns.Add("item3", typeof(long));
            table.Columns.Add("item4", typeof(long));
            table.Columns.Add("item5", typeof(long));
            table.Columns.Add("itemUnit1", typeof(string));
            table.Columns.Add("itemUnit2", typeof(string));
            table.Columns.Add("itemUnit3", typeof(string));
            table.Columns.Add("itemUnit4", typeof(string));
            table.Columns.Add("itemUnit5", typeof(string));
            table.Columns.Add("fetch_datetime", typeof(DateTime));
            table.Columns.Add("send_datetime", typeof(DateTime));
            table.Columns.Add("uuid", typeof(string));
            table.Columns.Add("flow_state", typeof(int));
            table.Columns.Add("error_code", typeof(int));
            table.Columns.Add("final_state", typeof(string));
            table.Columns.Add("result", typeof(string));
            table.Columns.Add("IsDelete", typeof(bool));
            table.Columns.Add("TaxId", typeof(string));

            foreach (var item in items)
            {
                table.Rows.Add(
                    item.WrapperId,
                    item.Indatim,
                    item.Inty,
                    item.Inno,
                    item.Inp,
                    item.Ins,
                    item.Tob,
                    item.Bid,
                    item.Bpc,
                    item.Radif,
                    item.BillId,
                    item.Sstid,
                    item.Sstt,
                    item.Mu,
                    item.Am,
                    item.Fee,
                    item.Dis,
                    item.Town,
                    item.Date_Bed,
                    item.Item1,
                    item.Item2,
                    item.Item3,
                    item.Item4,
                    item.Item5,
                    item.ItemUnit1,
                    item.ItemUnit2,
                    item.ItemUnit3,
                    item.ItemUnit4,
                    item.ItemUnit5,
                    item.Fetch_DateTime ?? (object)DBNull.Value,
                    item.SendDateTime ?? (object)DBNull.Value,
                    item.Uuid ?? (object)DBNull.Value,
                    item.Flow_State,
                    item.Error_Code ?? (object)DBNull.Value,
                    item.Final_State ?? (object)DBNull.Value,
                    item.Result ?? (object)DBNull.Value,
                    item.IsDelete ?? (object)DBNull.Value,
                    item.TaxId ?? (object)DBNull.Value
                );
            }

            return table;
        }
        //						--AbAndFazelab.dbo.PersianToUnix(@ToDateJalali) indatim,
        //						--AbAndFazelab.dbo.PersianToUnix(@ToDateJalali) indatim, 

        private string GetTaxQuery()
        {
            return @"Select top 100
						@WrapperId WrapperId,
						100000 indatim,
						'TYPE1' inty,
						CAST((z.UniqueCode+ FORMAT(c.CustomerNumber ,'0000000#') +FORMAT(0,'000000#')) AS bigint) inno,
						'SERVICE_BILLING' inp,
						'ORIGINAL' ins,
						'NATURAL' tob,
						c.NationalId bid , 
						c.PostalCode bpc, 
						b.CustomerNumber radif ,
						b.BillId billId,
						'2720000019557' sstid,
						N'آب بها با مصرف'+ N' '+CAST(b.Consumption AS varchar(20)) sstt, 
						'1647' mu,
						CAST(b.Consumption AS int) am,
					    CAST((b.Item1/IIF(b.Consumption=0,1,b.Consumption)) AS bigint) fee, 
						0 dis, 
						b.ZoneId town, 
						b.RegisterDay date_bed,
						b.Item1+b.Item11 item1,
						b.Item2 item2,
						b.Item3 item3, 
						b.Item4 item4, 
						b.Item18 item5,
						1653 itemUnit1, 
						1653 itemUnit2, 
						1653 itemUnit3, 
						1653 itemUnit4, 
						1653 itemUnit5,
						NULL fetch_datetime ,
						NULL [send_datetime], 
						CAST('' as char(36)) uuid, 
						1 flow_state, 
						NULL error_code
					From CustomerWarehouse.dbo.Clients c
					Join CustomerWarehouse.dbo.Bills b
						On c.ZoneId=b.ZoneId AND c.CustomerNumber=b.CustomerNumber
					Join Db70.dbo.T51 z
						On c.ZoneId=z.C0
					Where 
						c.ToDayJalali IS NULL AND
						b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali AND
						c.BillId <> '' AND
						b.Payable > 0 AND
						TRIM(b.BillId) <> '' AND
						(b.Item1+ Item11+Item2+Item3+Item4)>0 AND
						b.CounterStateCode NOT IN (4,7,8) 
					Union All
					select top 100 
						@WrapperId WrapperId,
						100000 indatim, 
						'TYPE1' inty,
						CAST((z.UniqueCode+ FORMAT(c.CustomerNumber ,'0000000#') +FORMAT(0,'000000#')) AS bigint) inno, 
						'SERVICE_BILLING' inp, 
						'ORIGINAL' ins, 
						'NATURAL' tob,
						c.NationalId bid ,
						c.PostalCode bpc, 
						b.CustomerNumber radif ,
						C.BillId billId,
						'2720000019557' sstid,
						N'حق انشعاب' sstt, 
						'1647' mu,
						0 am,
					    0 fee, 
						0 dis, 
						b.ZoneId town,
						b.RegisterDate date_bed,
						0 item1,
						0 item2, 
						0 item3, 
						0 item4,
						b.Amount item5,
						1653 itemUnit1, 
						1653 itemUnit2,
						1653 itemUnit3, 
						1653 itemUnit4,
						1653 itemUnit5,
						NULL fetch_datetime ,
						NULL [send_datetime],
						CAST('' as char(36)) uuid,
						1 flow_state, 
						NULL error_code
					from CustomerWarehouse.dbo.Clients c
					join CustomerWarehouse.dbo.RequestBillDetails b
					on c.ZoneId=b.ZoneId AND c.CustomerNumber=b.CustomerNumber
					join Db70.dbo.T51 z
					on b.ZoneId=z.C0
					where  
						c.ToDayJalali IS NULL AND
						b.RegisterDate BETWEEN @FromDateJalali AND @ToDateJalali AND
						c.BillId <>'' AND
						Amount>0 AND
						ItemId in (550)";
        }
        private string GetAmountAndCountQuery()
        {
            return @"Select 
						COUNT(1) as Count,
						SUM(item1+item2+item3+item4+item5) as SumAmount
					From Aban360.TaxPool.MaliatMaaherDetail 
					Where 
						WrapperId=@wrapperId
					Group By WrapperId";
        }
    }
}
