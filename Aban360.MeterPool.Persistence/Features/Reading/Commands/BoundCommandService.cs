using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace Aban360.MeterPool.Persistence.Features.Reading.Commands
{
    public sealed class BoundCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        private const string DestinationTableName = "[Atlas].[Reading].[Bound]";
        public BoundCommandService(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(BoundInsertDto input)
        {
            string command = GetInsertCommand();
            int recordEffected = await _connection.ExecuteAsync(command, input, _transaction);
            if (recordEffected != 1)
            {
                throw new ReadingException(ExceptionLiterals.InvalicZoneIdWithBlock);
            }
        }
        public async Task Insert(IEnumerable<BoundInsertDto> input)
        {
            var dataTable = ToDataTable(input);
            if (dataTable.Rows.Count == 0)
                return;

            using var bulkCopy = new SqlBulkCopy((SqlConnection)_connection, SqlBulkCopyOptions.Default, (SqlTransaction)_transaction)
            {
                DestinationTableName = DestinationTableName,
                BatchSize = 10000,
                BulkCopyTimeout = 0
            };

            foreach (DataColumn column in dataTable.Columns)
                bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);

            await bulkCopy.WriteToServerAsync(dataTable);
        }

        private static DataTable ToDataTable(IEnumerable<BoundInsertDto> input)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("ReadingBoundId", typeof(int));
            table.Columns.Add("ReadingBoundTitle", typeof(string));
            table.Columns.Add("ZoneId", typeof(int));
            table.Columns.Add("ZoneTitle", typeof(string));
            table.Columns.Add("FromReadingNumber", typeof(string));
            table.Columns.Add("ToReadingNumber", typeof(string));
            table.Columns.Add("IsValidated", typeof(bool));
            table.Columns.Add("BlockCount", typeof(int));
            table.Columns.Add("CustomerCount", typeof(int));
            table.Columns.Add("Year", typeof(int));
            table.Columns.Add("ReadingPeriodTypeId", typeof(int));
            table.Columns.Add("ReadingPeriodId", typeof(int));
            table.Columns.Add("InsertBy", typeof(Guid));
            table.Columns.Add("InsertDateTime", typeof(DateTime));

            var properties = typeof(BoundInsertDto).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead)
                .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

            foreach (var item in input)
            {
                var row = table.NewRow();
                foreach (DataColumn column in table.Columns)
                {
                    if (!properties.TryGetValue(column.ColumnName, out var property))
                        throw new InvalidOperationException($"Property '{column.ColumnName}' was not found on BoundInsertDto.");
                    row[column.ColumnName] = property.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }
        private string GetInsertCommand()
        {
            return $@"Insert Into Atlas.reading.Bound
                    (
                    	Id, ReadingBoundId, ReadingBoundTitle, ZoneId, ZoneTitle,
                    	FromReadingNumber, ToReadingNumber, IsValidated, BlockCount, CustomerCount,
                    	Year, ReadingPeriodTypeId, ReadingPeriodId,
                    	InsertBy, InsertDateTime
                    )
                    Values
                    (	
                        @Id, @ReadingBoundId, @ReadingBoundTitle, @ZoneId, @ZoneTitle,
                    	@FromReadingNumber, @ToReadingNumber, @IsValidated, @BlockCount, @CustomerCount,
                    	@Year, @ReadingPeriodTypeId, @ReadingPeriodId,
                    	@InsertBy, @InsertDateTime
                    );

                    Select SCOPE_IDENTITY();";
        }

    }

}
