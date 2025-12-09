using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;
using Aban360.TaxPool.Persistence.Features.MaaherTSP.Contracts;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.TaxPool.Persistence.Features.MaaherTSP.Implementations
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

                using var bulkCopy = new SqlBulkCopy(connection)
                {
                    DestinationTableName = "Aban360.TaxPool.MaliatMaaherDetail"
                };

                foreach (DataColumn col in table.Columns)
                    bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);

                await bulkCopy.WriteToServerAsync(table);
            };
        }

        public async Task<IEnumerable<MaliatMaaherDetailGetDto>> Get(MaliatMaaherDetailInsertBatchDto input)
        {
            string query = GetTaxQuery();
            IEnumerable<MaliatMaaherDetailGetDto> maaherDetail = await _sqlReportConnection.QueryAsync<MaliatMaaherDetailGetDto>(query, input);
            return maaherDetail;
        }
        public async Task<IEnumerable<MaliatMaaherDetailGetDto>> Get(int wrapperId)
        {
            string query = GetByWrapperIdQuery();
            IEnumerable<MaliatMaaherDetailGetDto> result = await _sqlConnection.QueryAsync<MaliatMaaherDetailGetDto>(query, new { wrapperId });
            if (result is null || !result.Any())
            {
                throw new TaxMaaherException(ExceptionLiterals.NotFoundAnyData);
            }

            return result;
        }
        public async Task<MaliatMaaherDetailAmountAndCountDto> GetAmountAndCount(int wrapperId)
        {
            string query = GetAmountAndCountQuery();
            MaliatMaaherDetailAmountAndCountDto result = await _sqlConnection.QueryFirstOrDefaultAsync<MaliatMaaherDetailAmountAndCountDto>(query, new { wrapperId });

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
                var row = table.NewRow();

                row["WrapperId"] = item.WrapperId;
                row["indatim"] = item.Indatim;
                row["inty"] = item.Inty;
                row["inno"] = item.Inno;
                row["inp"] = item.Inp;
                row["ins"] = item.Ins;
                row["tob"] = item.Tob;
                row["bid"] = item.Bid;
                row["bpc"] = item.Bpc;
                row["radif"] = item.Radif;
                row["billId"] = item.BillId;
                row["sstid"] = item.Sstid;
                row["sstt"] = item.Sstt;
                row["mu"] = item.Mu;
                row["am"] = item.Am;
                row["fee"] = item.Fee;
                row["dis"] = item.Dis;
                row["town"] = item.Town;
                row["date_bed"] = item.Date_Bed;
                row["item1"] = item.Item1;
                row["item2"] = item.Item2;
                row["item3"] = item.Item3;
                row["item4"] = item.Item4;
                row["item5"] = item.Item5;
                row["itemUnit1"] = item.ItemUnit1;
                row["itemUnit2"] = item.ItemUnit2;
                row["itemUnit3"] = item.ItemUnit3;
                row["itemUnit4"] = item.ItemUnit4;
                row["itemUnit5"] = item.ItemUnit5;
                row["fetch_datetime"] = item.Fetch_DateTime ?? (object)DBNull.Value;
                row["send_datetime"] = item.SendDateTime ?? (object)DBNull.Value;
                row["uuid"] = item.Uuid ?? (object)DBNull.Value;
                row["flow_state"] = item.Flow_State;
                row["error_code"] = item.Error_Code ?? (object)DBNull.Value;
                row["final_state"] = item.Final_State ?? (object)DBNull.Value;
                row["result"] = item.Result ?? (object)DBNull.Value;
                row["IsDelete"] = item.IsDelete ?? (object)DBNull.Value;
                row["TaxId"] = item.TaxId ?? (object)DBNull.Value;

                table.Rows.Add(row);

            }

            return table;
        }

        private string GetTaxQuery()//todo: review query
        {
            return @"Select top 100
						@WrapperId WrapperId,
                        --AbAndFazelab.dbo.PersianToUnix(@ToDateJalali) indatim,
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
                        --AbAndFazelab.dbo.PersianToUnix(@ToDateJalali) indatim,
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
        private string GetByWrapperIdQuery()
        {
            return @"Select *
                    From Aban360.TaxPool.MaliatMaaherDetail
                    Where WrapperId=@wrapperId";
        }
        private string GetAmountAndCountQuery()
        {
            return @"Select 
						COUNT(1) as InvoiceCount,
						SUM(item1+item2+item3+item4+item5) as SumAmount
					From Aban360.TaxPool.MaliatMaaherDetail 
					Where 
						WrapperId=@wrapperId
					Group By WrapperId";
        }
        
    }
}
