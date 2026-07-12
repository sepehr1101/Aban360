using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Persistence.Constants;
using Dapper;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    public sealed class WaterDebtCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public WaterDebtCommandService(
                IDbConnection connection,
                IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }


        public async Task UpdateAmount(string billId, long amount)
        {
            string command = GetUpdateAmountCommand();
            int recordCount = await _connection.ExecuteAsync(command, new { billId, amount }, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidCustomerCommandException(Exceptionliterals.InvalidUpdateWaterDebtAmount);
            }
        }
        public async Task UpdateAmount(ICollection<MembersFazelabCountAndDebtAmountUpdateDto> input)
        {
            //DataTable table = UpdateDebtAmountDataTable(input);
            //string tempTableCommand = GetUpdateDebtAmountCreateTmpTableCommand();
            //await _connection.ExecuteAsync(tempTableCommand, null, _transaction);

            //using var bulk = new SqlBulkCopy((SqlConnection)_connection, SqlBulkCopyOptions.Default, (SqlTransaction)_transaction)
            //{
            //    DestinationTableName = $"#temp",
            //    BatchSize = 5000,
            //    BulkCopyTimeout = 0
            //};

            //foreach (DataColumn col in table.Columns)
            //    bulk.ColumnMappings.Add(col.ColumnName, col.ColumnName);
            //await bulk.WriteToServerAsync(table);

            var check = await _connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM #DebtAmountUpdateTemp", null, _transaction);

            string updateCommand = GetUpdateDebtAmountCommand();
            int recordEffected = await _connection.ExecuteAsync(updateCommand, null, _transaction);
            if (recordEffected != (input?.Count() ?? 0))
            {
                throw new ReadingException(ExceptionLiterals.InvalidUpdateWaterDebt);
            }
        }
        private DataTable UpdateDebtAmountDataTable(IEnumerable<MembersFazelabCountAndDebtAmountUpdateDto> input)
        {
            DataTable table = new DataTable();

            table.Columns.Add("ZoneId", typeof(int));
            table.Columns.Add("CustomerNumber", typeof(int));
            table.Columns.Add("BillId", typeof(string));
            table.Columns.Add("Amount", typeof(long));

            foreach (var item in input)
            {
                table.Rows.Add(item.ZoneId, item.CustomerNumber, item.BillId, item.Amount);
            }
            return table;
        }
        private string GetUpdateDebtAmountCreateTmpTableCommand()
        {
            return $@"Create Table #temp
                    (
                    	ZoneId int Not Null,
                    	CustomerNumber  int Not Null,
                    	BillId  nvarchar(20) Not Null,
                    	Amount bigint Not Null
                    )";
        }
        private string GetUpdateDebtAmountCommand()
        {
            return $@"Update w
                    Set w.Debt=w.Debt+t.Amount
                    From CustomerWarehouse.dbo.WaterDebt w
                    Join #DebtAmountUpdateTemp t
                    	On TRIM(w.BillId) Collate SQL_Latin1_General_CP1_CI_AS=t.BillId Collate SQL_Latin1_General_CP1_CI_AS";
        }
        private string GetUpdateAmountCommand()
        {
            return $@"Update CustomerWarehouse.dbo.WaterDebt
                    Set Debt=Debt+@Amount
                    Where TRIM(BillId)=@billId";
        }
    }
}
