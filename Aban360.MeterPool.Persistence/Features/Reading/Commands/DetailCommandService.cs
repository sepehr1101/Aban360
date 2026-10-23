using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace Aban360.MeterPool.Persistence.Features.Reading.Commands
{
    public sealed class DetailCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        private const string DestinationTableName = "[Atlas].[Reading].[Detail]";
        public DetailCommandService(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(IEnumerable<DetailInsertDto> input)
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
        public async Task Update(IEnumerable<DetailUpdateDto> input)
        {
            string command = GetUpdateQuery();
            int recordEffected = await _connection.ExecuteAsync(command, input, _transaction);
            if (recordEffected != (input?.Count() ?? 0))
            {
                throw new ReadingException(ExceptionLiterals.InvalidUpdateDetail);
            }
        }
        private static DataTable ToDataTable(IEnumerable<DetailInsertDto> input)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("TrackingId", typeof(Guid));
            table.Columns.Add("TrackNumber", typeof(long));
            table.Columns.Add("BillId", typeof(string));
            table.Columns.Add("CustomerNumber", typeof(int));
            table.Columns.Add("ReadingNumber", typeof(string));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("SureName", typeof(string));
            table.Columns.Add("Address", typeof(string));
            table.Columns.Add("Plaque", typeof(string));
            table.Columns.Add("UsageId", typeof(int));
            table.Columns.Add("DomesticUnit", typeof(int));
            table.Columns.Add("CommercialUnit", typeof(int));
            table.Columns.Add("OtherUnit", typeof(int));
            table.Columns.Add("MeterDiameterId", typeof(int));
            table.Columns.Add("SiphonDiameterId", typeof(int));
            table.Columns.Add("PostalCode", typeof(string));
            table.Columns.Add("PreNumber", typeof(int));
            table.Columns.Add("PreDateJalali", typeof(string));
            table.Columns.Add("PreDateTime", typeof(DateTime));
            table.Columns.Add("PreConsumptionAverage", typeof(double));
            table.Columns.Add("PreCounterStateCode", typeof(int));
            table.Columns.Add("BodySerial", typeof(string));
            table.Columns.Add("MeterInstallationDateJalali", typeof(string));
            table.Columns.Add("ChangeDateJalali", typeof(string));
            table.Columns.Add("ChangeNumber", typeof(int));
            table.Columns.Add("ContractualCapacity", typeof(int));
            table.Columns.Add("MobileNumber", typeof(string));
            table.Columns.Add("DeletionStateId", typeof(int));
            table.Columns.Add("BranchTypeId", typeof(int));
            table.Columns.Add("CounterNumber", typeof(int));
            table.Columns.Add("CurrentCounterStateCode", typeof(int));
            table.Columns.Add("PossibleAddress", typeof(string));
            table.Columns.Add("PossibleBodySerial", typeof(string));
            table.Columns.Add("PossibleReadingNumber", typeof(string));
            table.Columns.Add("PossibleMobileNumber", typeof(string));
            table.Columns.Add("PossiblePhoneNumber", typeof(string));
            table.Columns.Add("PossibleDomesticUnit", typeof(int));
            table.Columns.Add("PossibleCommercialUnit", typeof(int));
            table.Columns.Add("PossibleOtherUnit", typeof(int));
            table.Columns.Add("PossibleUsageId", typeof(int));
            table.Columns.Add("PossibleEmptyUnit", typeof(int));
            table.Columns.Add("OffLoadDateTime", typeof(DateTime));
            table.Columns.Add("OffloadDateJalali", typeof(string));
            table.Columns.Add("OffLoadTime", typeof(string));
            table.Columns.Add("OffLoadBy", typeof(Guid));
            table.Columns.Add("Y", typeof(string));
            table.Columns.Add("X", typeof(string));
            table.Columns.Add("GisAccuracy", typeof(string));
            table.Columns.Add("ImageCount", typeof(int));
            table.Columns.Add("CurrentConsumption", typeof(int));
            table.Columns.Add("CurrentConsumptionAverage", typeof(float));
            table.Columns.Add("DateDifference", typeof(int));
            table.Columns.Add("CounterNumberShown", typeof(bool));
            table.Columns.Add("ExcludedForBazdid", typeof(bool));
            table.Columns.Add("ExcludedForEslah", typeof(bool));
            table.Columns.Add("JsonItems", typeof(string));
            table.Columns.Add("ConsumptionStateId", typeof(int));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("IsCalculated", typeof(bool));
            table.Columns.Add("CalculationErrorDescription", typeof(string));
            table.Columns.Add("D1", typeof(string));
            table.Columns.Add("D2", typeof(string));
            table.Columns.Add("CounterStatePosition", typeof(int));
            table.Columns.Add("AttemptCount", typeof(int));
            table.Columns.Add("IsLocked", typeof(bool));
            table.Columns.Add("LocationDateTime", typeof(string));
            table.Columns.Add("PhoneDateTime", typeof(string));
            table.Columns.Add("FatherName", typeof(string));
            table.Columns.Add("OldReadingNumber", typeof(string));
            table.Columns.Add("OldCustomerNumber", typeof(string));
            table.Columns.Add("MobilesNumber", typeof(string));
            table.Columns.Add("WaterDebt", typeof(long));
            table.Columns.Add("ModifierUserId", typeof(Guid));
            table.Columns.Add("GuildId", typeof(int));
            table.Columns.Add("SiphonInstallationDateJalali", typeof(string));
            table.Columns.Add("PreGuildId", typeof(int));
            table.Columns.Add("HighLowStateId", typeof(int));
            table.Columns.Add("ExcludeBy", typeof(Guid));
            table.Columns.Add("ExcludeDateTime", typeof(DateTime));
            table.Columns.Add("ExcludeCauseId", typeof(int));
            table.Columns.Add("ExcludeCauseTitle", typeof(string));

            var properties = typeof(DetailInsertDto).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead)
                .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

            foreach (var item in input)
            {
                var row = table.NewRow();
                foreach (DataColumn column in table.Columns)
                {
                    if (!properties.TryGetValue(column.ColumnName, out var property))
                        throw new InvalidOperationException($"Property '{column.ColumnName}' was not found on DetailInsertDto.");
                    row[column.ColumnName] = property.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }
        private string GetUpdateQuery()
        {
            return $@"Update Atlas.reading.Detail
                    Set
                    	CounterNumber = @CounterNumber,
                    	LocationDateTime = @LocationDateTime,
                    	PhoneDateTime = @PhoneDateTime,
                    	CurrentCounterStateCode = @CurrentCounterStateCode,
                    	Y = @Y,
                    	X = @X,
                    	GisAccuracy = @GisAccuracy,
                    	CounterNumberShown = @CounterNumberShown,
                    	Description = @Description,
                    	AttemptCount = @AttemptCount,
                    	IsLocked = @IsLocked,
                    	HighLowStateId = @HighLowStateId,
                    	OffLoadDateTime = @OffLoadDateTime,
                    	OffloadDateJalali = @OffloadDateJalali,
                    	OffLoadTime = @OffLoadTime,
                    	OffLoadBy = @OffLoadBy 
                    Where Id = @Id";
        }
    }
}

