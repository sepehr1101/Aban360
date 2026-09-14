using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.CommunicationPool.Domain.Features.Sms.Commands;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Aban360.CommunicationPool.Persistence.Features.Sms.Commands.Implementations
{
    public sealed class SmsDraftCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        private string _tempIdTable = "#SmsDraftUpdateDateTemp";
        public SmsDraftCommandService(
            IDbConnection sqlRonnection,
            IDbTransaction transaction)
        {
            _connection = sqlRonnection;
            _connection.NotNull(nameof(sqlRonnection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(SmsDraftInsertDto inputDto)
        {
            string command = GetInsertQuery();
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidSmsDraftInsert);
            }
        }
        public async Task Insert(IEnumerable<SmsDraftInsertDto> inputDtos)
        {
            DataTable table = GetInsertDataTable(inputDtos);
            using var bulk = new SqlBulkCopy((SqlConnection)_connection, SqlBulkCopyOptions.Default, (SqlTransaction)_transaction)
            {
                DestinationTableName = "[Atlas].dbo.SmsDraft",
                BatchSize = 1000,
                BulkCopyTimeout = 0,
            };

            foreach (DataColumn column in table.Columns)
            {
                bulk.ColumnMappings.Add(column.ColumnName, column.ColumnName);
            }
            await bulk.WriteToServerAsync(table);
        }
        public async Task Update(SmsDraftUpdateDto input, bool isUpdateFetchDate)
        {
            DataTable table = GetUpdateDataTable(input.Ids);
            string createTempTable = @$"Create Table {_tempIdTable} ( Id uniqueidentifier NOT NULL )";
            await _connection.ExecuteAsync(createTempTable, null, _transaction);
            using var bulk = new SqlBulkCopy((SqlConnection)_connection, SqlBulkCopyOptions.Default, (SqlTransaction)_transaction)
            {
                DestinationTableName = _tempIdTable,
                BatchSize = 5000,
                BulkCopyTimeout = 0
            };
            await bulk.WriteToServerAsync(table);

            string updateCommand = GetUpdateQuery(isUpdateFetchDate);
            int rowEffect = await _connection.ExecuteAsync(updateCommand, input, _transaction);
            if (rowEffect != (input.Ids?.Count() ?? 0))
            {
                throw new ReadingException(ExceptionLiterals.InvalidSmsDraftUpdate);
            }
        }

        private DataTable GetUpdateDataTable(IEnumerable<Guid> ids)
        {
            var table = new DataTable();

            table.Columns.Add("Id", typeof(Guid));
            foreach (var id in ids)
            {
                table.Rows.Add(id);
            }
            return table;
        }
        private DataTable GetInsertDataTable(IEnumerable<SmsDraftInsertDto> inputDtos)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("TrackNumber", typeof(int));
            table.Columns.Add("BillId", typeof(string));
            table.Columns.Add("GroupId", typeof(string));
            table.Columns.Add("ReferenceId", typeof(string));
            table.Columns.Add("SmsFlowId", typeof(int));
            table.Columns.Add("Message", typeof(string));
            table.Columns.Add("MobileNumber", typeof(string));
            table.Columns.Add("InsertDateTime", typeof(DateTime));
            table.Columns.Add("FetchDateTime", typeof(DateTime));
            table.Columns.Add("SendDateTime", typeof(DateTime));

            foreach (var item in inputDtos)
            {
                var row = table.NewRow();

                row["Id"] = item.Id;
                row["TrackNumber"] = item.TrackNumber ?? (object)DBNull.Value;
                row["BillId"] = item.BillId ?? (object)DBNull.Value;
                row["GroupId"] = item.GroupId;
                row["ReferenceId"] = item.ReferenceId;
                row["SmsFlowId"] = item.SmsFlowId;
                row["Message"] = item.Message;
                row["MobileNumber"] = item.MobileNumber;
                row["InsertDateTime"] = item.InsertDateTime;
                row["FetchDateTime"] = item.FetchDateTime ?? (object)DBNull.Value;
                row["SendDateTime"] = item.SendDateTime ?? (object)DBNull.Value;

                table.Rows.Add(row);
            }

            return table;
        }
        private string GetInsertQuery()
        {
            return @"Insert Into [Atlas].dbo.SmsDraft
                    (
                    	Id,TrackNumber, BillId,
                    	GroupId, ReferenceId, SmsFlowId, 
                        Message, MobileNumber,
                    	InsertDateTime, FetchDateTime, SendDateTime
                    )
                    Values
                    (
                    	@Id, @TrackNumber, @BillId,
                    	@GroupId, @ReferenceId, @SmsFlowId, 
                        @Message, @MobileNumber,
                    	@InsertDateTime, @FetchDateTime, @SendDateTime
                    )";
        }
        private string GetUpdateQuery(bool isFetchDate)
        {
            string dateProperty = isFetchDate ? "s.FetchDateTime" : "s.SendDateTime";
            return $@"Update s
                    Set
                    	{dateProperty} = @EffectDateTime
                    From [Atlas].dbo.SmsDraft s
                    Join {_tempIdTable} tempS
                    	On s.Id = tempS.Id 
                    Where
                        s.GroupId = @groupId AND
                        s.SmsFlowId = @smsFlowId AND
                    	{dateProperty} IS NULL";
        }
    }
}
