using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Constants;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    public sealed class ContorCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public ContorCommandService(
                IDbConnection connection,
                IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Update(ContorUpdateDto inputDto, string dbName, bool isUpdateTavizField)
        {
            string command = GetUpdateCommand(dbName, isUpdateTavizField);
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidBillCommandException(Exceptionliterals.InvalidControUpdate);
            }
        }
        public async Task UpdateMeterChange(ContorMeterChangeUpdateDto inputDto, string dbName)
        {
            string command = GetUpdateMeterChangeCommand(dbName);
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidBillCommandException(Exceptionliterals.InvalidInsertMeterChange);
            }
        }
        public async Task Update(ICollection<ContorUpdateDto> inputDto, string dbName, bool isUpdateTavizField)
        {
            DataTable updateDataTable = GetUpdateDataTable(inputDto);
            string updateTempTableCommand = GetUpdateTempTableCommand();
            await _connection.ExecuteAsync(updateTempTableCommand, null, _transaction);

            using var bulk = new SqlBulkCopy((SqlConnection)_connection, SqlBulkCopyOptions.Default, (SqlTransaction)_transaction)
            {
                DestinationTableName = "#ContorTemp",
                BatchSize = 5000,
                BulkCopyTimeout = 0
            };
            foreach (DataColumn col in updateDataTable.Columns)
                bulk.ColumnMappings.Add(col.ColumnName, col.ColumnName);

            await bulk.WriteToServerAsync(updateDataTable);
            IEnumerable<int> check = await _connection.QueryAsync<int>("SELECT CustomerNumber  FROM #ContorTemp", null, _transaction);

            string updateCommand = GetUpdateWithTempTableCommand(dbName, isUpdateTavizField);
            int recordEffected = await _connection.ExecuteAsync(updateCommand, null, _transaction);
            if(recordEffected != (inputDto?.Count() ?? 0))
            {
                throw new ReadingException(ExceptionLiterals.InvalidUpdateContor);
            }
        }

        private DataTable GetUpdateDataTable(ICollection<ContorUpdateDto> input)
        {
            DataTable table = new DataTable();

            table.Columns.Add("ZoneId", typeof(int));
            table.Columns.Add("CustomerNumber", typeof(int));
            table.Columns.Add("CurrentDateJalali", typeof(string));
            table.Columns.Add("CurrentNumber", typeof(int));
            table.Columns.Add("Consumption", typeof(int));
            table.Columns.Add("ConsumptionAverage", typeof(float));
            table.Columns.Add("PreviousCounterState", typeof(int));

            foreach (var item in input)
            {
                table.Rows.Add(item.ZoneId, item.CustomerNumber, item.CurrentDateJalali, item.CurrentNumber, item.Consumption, item.ConsumptionAverage, item.PreviousCounterState);
            }
            return table;
        }
        private string GetUpdateTempTableCommand()
        {
            return @"Create Table #ContorTemp
                    (
                        ZoneId Int,
                        CustomerNumber Int,
                        CurrentDateJalali nvarchar(10),
                        CurrentNumber Int,
                        Consumption Int,
                        ConsumptionAverage Float,
                        PreviousCounterState Int
                    )";
        }
        private string GetUpdateWithTempTableCommand(string dbName, bool isUpdateTavizField)
        {
            string tavizUpdate = isUpdateTavizField ?
                @"taviz_date=t.MeterChangeDateJalali,
                  taviz_no=t.MeterChangeNumber," :
                string.Empty;

            return $@"Update c
                    Set
                    	c.pri_no=t.CurrentNumber,
                    	c.today_no=0,
                    	c.pri_date=t.CurrentDateJalali,
                    	c.today_date='',
                    	{tavizUpdate}
                        c.masraf=t.Consumption,
                    	c.cod_vas=0,
                    	c.average=t.ConsumptionAverage,
                    	c.mohasbat=2,
                    	c.operator=5,
                    	c.mamor=0,
                    	c.cod_report=0,
                    	c.old_vas=t.PreviousCounterState,
                    	c.eslah=0
                    From [{dbName}].dbo.contor c
                    Join #ContorTemp t
                    	ON c.town=t.ZoneId AND c.radif=t.CustomerNumber  ";
        }
        private string GetUpdateCommand(string dbName, bool isUpdateTavizField)
        {
            string tavizUpdate = isUpdateTavizField ?
                @"taviz_date=@MeterChangeDateJalali,
                  taviz_no=@MeterChangeNumber," :
                string.Empty;

            return $@"Update [{dbName}].dbo.contor
                    Set
                    	pri_no=@CurrentNumber,
                    	today_no=0,
                    	pri_date=@CurrentDateJalali,
                    	today_date='',
                    	masraf=@Consumption,
                    	{tavizUpdate}
                    	cod_vas=0,
                    	average=@ConsumptionAverage,
                    	mohasbat=2,
                    	operator=5,
                    	mamor=0,
                    	cod_report=0,
                    	old_vas=@PreviousCounterState,
                    	eslah=0
                    Where town=@ZoneId AND radif=@CustomerNumber  ";
        }
        private string GetUpdateMeterChangeCommand(string dbName)
        {
            return $@"Update [{dbName}].dbo.contor
                    Set 
                    	taviz_date=@MeterChangeDateJalali , 
                    	taviz_no=@MeterChangeNumber
                    Where 
                    	town=@ZoneId AND
                    	radif=@CustomerNumber";
        }
    }
}
