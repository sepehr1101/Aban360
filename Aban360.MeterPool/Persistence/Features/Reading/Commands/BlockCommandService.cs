using Aban360.Common.Extensions;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Commands;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

public sealed class BlockCommandService
{
    private readonly IDbConnection _connection;
    private readonly IDbTransaction _transaction;
    private const string DestinationTableName = "[Atlas].[Reading].[Block]";
    public BlockCommandService(
        IDbConnection connection,
        IDbTransaction transaction)
    {
        _connection = connection;
        _connection.NotNull(nameof(connection));

        _transaction = transaction;
        _transaction.NotNull(nameof(transaction));
    }

    public async Task Insert(IEnumerable<BlockInsertDto> input)
    {
        var dataTable = ToDataTable(input);
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

    private static DataTable ToDataTable(IEnumerable<BlockInsertDto> input)
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(Guid));
        table.Columns.Add("BoundId", typeof(Guid));
        table.Columns.Add("ReadingBlockId", typeof(int));
        table.Columns.Add("ReadingBlockTitle", typeof(string));
        table.Columns.Add("FromReadingNumber", typeof(string));
        table.Columns.Add("ToReadingNumber", typeof(string));
        table.Columns.Add("CustomerCount", typeof(int));
        table.Columns.Add("OrderDigit", typeof(int));
        table.Columns.Add("OrderPersian", typeof(string));

        var properties = typeof(BlockInsertDto).GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanRead)
            .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

        foreach (var item in input)
        {
            var row = table.NewRow();
            foreach (DataColumn column in table.Columns)
            {
                if (!properties.TryGetValue(column.ColumnName, out var property))
                    throw new InvalidOperationException($"Property '{column.ColumnName}' was not found on BlockInsertDto.");
                row[column.ColumnName] = property.GetValue(item) ?? DBNull.Value;
            }
            table.Rows.Add(row);
        }

        return table;
    }
}
