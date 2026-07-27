using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Features.Tagging;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Commands
{
    public class BillIdTagCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public BillIdTagCommandService(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task<long> Create(CreateBillIdTagDto input)
        {
            string command = GetInsertCommand();
            return await _connection.ExecuteScalarAsync<long>(command, input, _transaction);
        }
        public async Task Create(ICollection<BillIdTagByStringCodeDto> input)
        {
            DataTable table = await CompleteAndGetBillIdTagTemplateTable(input);
            using (var bulk = new SqlBulkCopy((SqlConnection)_connection, SqlBulkCopyOptions.Default, (SqlTransaction)_transaction))
            {
                bulk.DestinationTableName = "#BillIdTagTemplate";
                bulk.BatchSize = 10000;
                await bulk.WriteToServerAsync(table);
            };

            string command = GetInsertByStringCodeCommand();
            int effectedRecords = await _connection.ExecuteAsync(command, null, _transaction);
            if (effectedRecords != (input?.Count() ?? 0))
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidInsertBillIdTag);
            }
        }
        public async Task<bool> Delete(long id)
        {
            var command = GetDeleteCommand();
            var effectedRecords = await _connection.ExecuteAsync(command, new { Id = id }, _transaction);
            return effectedRecords > 0;
        }
        public async Task Delete(IEnumerable<int> tagIds)
        {
            var command = GetDeleteByTagIdsCommand();
            int effectedRecords = await _connection.ExecuteAsync(command, new { tagIds }, _transaction);
        }

        private async Task<DataTable> CompleteAndGetBillIdTagTemplateTable(ICollection<BillIdTagByStringCodeDto> input)
        {
            DataTable table = new DataTable();
            table.Columns.Add("BillId", typeof(string));
            table.Columns.Add("StringCode", typeof(string));
            table.Columns.Add("ExpireDateJalali", typeof(string));
            foreach (var item in input)
            {
                DataRow row = table.NewRow();

                row["BillId"] = item.BillId;
                row["StringCode"] = item.StringCode;
                row["ExpireDateJalali"] = item.ExpireDateJalali ?? (object)DBNull.Value;

                table.Rows.Add(row);
            }

            string createTemplateTable = @"Create Table #BillIdTagTemplate
                                            (
                                                BillId VARCHAR(15) NOT NULL ,
                                                StringCode VARCHAR(15) NOT NULL,
                                                ExpireDateJalali CHAR(10) NULL
                                            )";
            await _connection.ExecuteAsync(createTemplateTable, null, _transaction);
            return table;
        }
        private string GetInsertCommand()
        {
            return $@"INSERT INTO [CustomerWarehouse].dbo.BillIdTags (BillId, TagId, TagTitle, ExpireDateJalali, CreateDateTime)
                SELECT @BillId, @TagId, t.Title, @ExpireDateJalali, GETUTCDATE()  
                FROM [CustomerWarehouse].dbo.Tags t
                WHERE t.Id = @TagId;
                SELECT CAST(SCOPE_IDENTITY() as bigint);";
        }
        private string GetInsertByStringCodeCommand()
        {
            return $@"INSERT INTO [CustomerWarehouse].dbo.BillIdTags (BillId, TagId, TagTitle, ExpireDateJalali, CreateDateTime)
                    SELECT bt.BillId, t.Id, t.Title, bt.ExpireDateJalali, GETUTCDATE()  
                    FROM [CustomerWarehouse].dbo.Tags t
                    JOIN #BillIdTagTemplate bt
                        ON bt.StringCode COLLATE Persian_100_CI_AI=t.StringCode COLLATE Persian_100_CI_AI;";
        }
        private string GetDeleteCommand()
        {
            return $@"Update CustomerWarehouse.dbo.BillIdTags
                    Set DeleteDateTime = GETDATE()
                    Where Id=@Id";
        }
        private string GetDeleteByTagIdsCommand()
        {
            return $@"Update CustomerWarehouse.dbo.BillIdTags
                    Set DeleteDateTime = GETDATE()
                    Where TagId IN @TagIds";
        }
    }
}
