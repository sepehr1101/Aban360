using Aban360.Common.Extensions;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace Aban360.MeterPool.Persistence.Features.Reading.Commands
{
    public sealed class TrackingCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        private const string DestinationTableName = "[Atlas].[Reading].[Tracking]";
        public TrackingCommandService(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(IEnumerable<TrackingGetDto> input)
        {
            ArgumentNullException.ThrowIfNull(input);
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

        private static DataTable ToDataTable(IEnumerable<TrackingGetDto> input)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("BlockId", typeof(Guid));
            table.Columns.Add("TrackNumber", typeof(long));
            table.Columns.Add("TbReadingProgramId", typeof(Guid));
            table.Columns.Add("ZoneId", typeof(int));
            table.Columns.Add("ZoneTitle", typeof(string));
            table.Columns.Add("FlowStepId", typeof(int));
            table.Columns.Add("FromReadingNumber", typeof(string));
            table.Columns.Add("ToReadingNumber", typeof(string));
            table.Columns.Add("ListNumber", typeof(string));
            table.Columns.Add("InsertBy", typeof(Guid));
            table.Columns.Add("InsertDateJalali", typeof(string));
            table.Columns.Add("InsertDateTime", typeof(DateTime));
            table.Columns.Add("Seen", typeof(bool));
            table.Columns.Add("CounterReaderCode", typeof(int));
            table.Columns.Add("CounterReaderId", typeof(Guid));
            table.Columns.Add("CounterReaderName", typeof(string));
            table.Columns.Add("CounterReaderMobile", typeof(string));
            table.Columns.Add("IsBazdid", typeof(bool));
            table.Columns.Add("IsRoosta", typeof(bool));
            table.Columns.Add("FromDate", typeof(string));
            table.Columns.Add("ToDate", typeof(string));
            table.Columns.Add("OverallCustomerCount", typeof(int));
            table.Columns.Add("ItemCustomerCount", typeof(int));
            table.Columns.Add("ProvisionallyPercent", typeof(int));
            table.Columns.Add("ImagePercent", typeof(int));
            table.Columns.Add("HasPreNumber", typeof(bool));
            table.Columns.Add("DisplayBillId", typeof(bool));
            table.Columns.Add("DisplayCustomerNumber", typeof(bool));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("X", typeof(string));
            table.Columns.Add("Y", typeof(string));
            table.Columns.Add("DisplayPreDate", typeof(bool));
            table.Columns.Add("DisplayMobile", typeof(bool));
            table.Columns.Add("HasImage", typeof(bool));
            table.Columns.Add("DisplayDebt", typeof(bool));
            table.Columns.Add("DisplayIcons", typeof(bool));

            var properties = typeof(TrackingGetDto).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead)
                .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

            foreach (var item in input)
            {
                var row = table.NewRow();
                foreach (DataColumn column in table.Columns)
                {
                    if (!properties.TryGetValue(column.ColumnName, out var property))
                        throw new InvalidOperationException($"Property '{column.ColumnName}' was not found on TrackingGetDto.");
                    row[column.ColumnName] = property.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }
    }

}
