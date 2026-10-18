using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

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

        public async Task Insert(IEnumerable<TrackingInsertDto> input)
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
        public async Task InsertDuplicate(TrackingDuplicateInsertDto input)
        {
            string command = GetInsertDuplicateCommand();
            int recordEffected = await _connection.ExecuteAsync(command, input, _transaction);
            if (recordEffected != (input.TrackingIds?.Count() ?? 0))
            {
                throw new ReadingException(ExceptionLiterals.InvalidTrackingUpdateRecord);
            }
        }

        private static DataTable ToDataTable(IEnumerable<TrackingInsertDto> input)
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

            foreach (var item in input)
            {
                var row = table.NewRow();

                row["Id"] = item.Id;
                row["BlockId"] = item.BlockId;
                row["TrackNumber"] = item.TrackNumber;
                row["TbReadingProgramId"] = item.TbReadingProgramId ?? (object)DBNull.Value;
                row["ZoneId"] = item.ZoneId;
                row["ZoneTitle"] = item.ZoneTitle;
                row["FlowStepId"] = item.FlowStepId;
                row["FromReadingNumber"] = item.FromReadingNumber;
                row["ToReadingNumber"] = item.ToReadingNumber;
                row["ListNumber"] = (object?)item.ListNumber ?? DBNull.Value;
                row["InsertBy"] = item.InsertBy;
                row["InsertDateJalali"] = item.InsertDateJalali;
                row["InsertDateTime"] = item.InsertDateTime;
                row["Seen"] = item.Seen;
                row["CounterReaderCode"] = item.CounterReaderCode;
                row["CounterReaderId"] = item.CounterReaderId;
                row["CounterReaderName"] = item.CounterReaderName;
                row["CounterReaderMobile"] = item.CounterReaderMobile;
                row["IsBazdid"] = item.IsBazdid;
                row["IsRoosta"] = (object?)item.IsRoosta ?? DBNull.Value;
                row["FromDate"] = (object?)item.FromDate ?? DBNull.Value;
                row["ToDate"] = (object?)item.ToDate ?? DBNull.Value;
                row["OverallCustomerCount"] = item.OverallCustomerCount;
                row["ItemCustomerCount"] = item.ItemCustomerCount;
                row["ProvisionallyPercent"] = item.ProvisionallyPercent;
                row["ImagePercent"] = item.ImagePercent;
                row["HasPreNumber"] = item.HasPreNumber;
                row["DisplayBillId"] = item.DisplayBillId;
                row["DisplayCustomerNumber"] = item.DisplayCustomerNumber;
                row["Description"] = (object?)item.Description ?? DBNull.Value;
                row["X"] = (object?)item.X ?? DBNull.Value;
                row["Y"] = (object?)item.Y ?? DBNull.Value;
                row["DisplayPreDate"] = (object?)item.DisplayPreDate ?? DBNull.Value;
                row["DisplayMobile"] = (object?)item.DisplayMobile ?? DBNull.Value;
                row["HasImage"] = (object?)item.HasImage ?? DBNull.Value;
                row["DisplayDebt"] = (object?)item.DisplayDebt ?? DBNull.Value;
                row["DisplayIcons"] = (object?)item.DisplayIcons ?? DBNull.Value;

                table.Rows.Add(row);
            }
            return table;
        }
        private string GetInsertDuplicateCommand()
        {
            return $@"Insert Into [Atlas].reading.Tracking
                    Select 
                    	NEWID(),
                    	BlockId ,
                    	TrackNumber ,
                    	TbReadingProgramId,
                    	ZoneId ,
                    	ZoneTitle ,
                    	@FlowStepId FlowStepId ,
                    	FromReadingNumber ,
                    	ToReadingNumber ,
                    	ListNumber ,
                    	@InsertBy InsertBy ,
                    	@InsertDateJalali InsertDateJalali ,
                    	@InsertDateTime InsertDateTime ,
                    	Seen ,
                    	CounterReaderCode ,
                    	CounterReaderId ,
                    	CounterReaderName ,
                    	CounterReaderMobile ,
                    	IsBazdid ,
                    	IsRoosta,
                    	FromDate,
                    	ToDate,
                    	OverallCustomerCount,
                    	ItemCustomerCount,
                    	ProvisionallyPercent,
                    	ImagePercent,
                    	HasPreNumber,
                    	DisplayBillId,
                    	DisplayCustomerNumber,
                    	Description,
                    	X,
                    	Y,
                    	DisplayPreDate,
                    	DisplayMobile,
                    	HasImage,
                    	DisplayDebt,
                    	DisplayIcons
                    From [Atlas].reading.Tracking
                    Where Id IN @TrackingIds";
        }
    }
}
