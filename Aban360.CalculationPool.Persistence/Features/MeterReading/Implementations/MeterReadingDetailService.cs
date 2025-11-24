using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using LiteDB;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Implementations
{
    internal sealed class MeterReadingDetailService : AbstractBaseConnection, IMeterReadingDetailService
    {
        public MeterReadingDetailService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Create(IEnumerable<MeterReadingDetailCreateDto> input)
        {
            var dataTable = ToDataTable(input);

            using (var sqlConnection = _sqlReportConnection)
            {
                await sqlConnection.OpenAsync();

                using (var bulkCopy = new SqlBulkCopy(sqlConnection))
                {
                    bulkCopy.DestinationTableName = "[Atlas].dbo.MeterReadingDetail";

                    foreach (DataColumn col in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }

                    bulkCopy.BatchSize = 10000;
                    bulkCopy.BulkCopyTimeout = 0;

                    await bulkCopy.WriteToServerAsync(dataTable);
                }
            }
        }
        public async Task Update(IEnumerable<MeterReadingWithAbBahaResultUpdateDto> items)
        {
            var dt = ToDataTable(items);

            using var connection = _sqlReportConnection;
            await connection.OpenAsync();

            string createTemp = @"
                    CREATE TABLE #TempMeterReadingDetail
                    (
                        Id INT PRIMARY KEY,
                        SumItems FLOAT(53) NULL,
                        SumItemsBeforeDiscount FLOAT(53) NULL,
                        DiscountSum FLOAT(53) NULL,
                        Consumption FLOAT(53) NULL,
                        MonthlyConsumption FLOAT(53) NULL
                    );";

            await new SqlCommand(createTemp, connection).ExecuteNonQueryAsync();

            using (var bulk = new SqlBulkCopy(connection))
            {
                bulk.DestinationTableName = "#TempMeterReadingDetail";
                await bulk.WriteToServerAsync(dt);
            }

            string mergeSql = @"
                    MERGE Atlas.dbo.MeterReadingDetail AS target
                    USING #TempMeterReadingDetail AS source
                    ON target.Id = source.Id
                    WHEN MATCHED THEN 
                        UPDATE SET
                            target.SumItems = source.SumItems,
                            target.SumItemsBeforeDiscount = source.SumItemsBeforeDiscount,
                            target.DiscountSum = source.DiscountSum,
                            target.Consumption = source.Consumption,
                            target.MonthlyConsumption = source.MonthlyConsumption;";

            await new SqlCommand(mergeSql, connection).ExecuteNonQueryAsync();
        }


        public async Task<IEnumerable<MeterReadingDetailGetDto>> Get(int flowImportedId)
        {
            string query = GetQuery();
            IEnumerable<MeterReadingDetailGetDto> details = await _sqlReportConnection.QueryAsync<MeterReadingDetailGetDto>(query, new { flowImportedId = flowImportedId });

            return details;
        }

        private DataTable ToDataTable(IEnumerable<MeterReadingDetailCreateDto> input)
        {
            var table = new DataTable();

            table.Columns.Add("FlowImportedId", typeof(int));
            table.Columns.Add("ZoneId", typeof(int));
            table.Columns.Add("CustomerNumber", typeof(int));
            table.Columns.Add("ReadingNumber", typeof(string));
            table.Columns.Add("BillId", typeof(string));
            table.Columns.Add("AgentCode", typeof(int));
            table.Columns.Add("CurrentCounterStateCode", typeof(short));
            table.Columns.Add("PreviousDateJalali", typeof(string));
            table.Columns.Add("CurrentDateJalali", typeof(string));
            table.Columns.Add("PreviousNumber", typeof(int));
            table.Columns.Add("CurrentNumber", typeof(int));
            table.Columns.Add("InsertByUserId", typeof(Guid));
            table.Columns.Add("InsertDateTime", typeof(DateTime));
            table.Columns.Add("RemovedByUserId", typeof(Guid));
            table.Columns.Add("RemovedDateTime", typeof(DateTime));
            table.Columns.Add("ExcludedByUserId", typeof(Guid));
            table.Columns.Add("ExcludedDateTime", typeof(DateTime));

            table.Columns.Add("UsageId", typeof(int));
            table.Columns.Add("DomesticUnit", typeof(int));
            table.Columns.Add("CommercialUnit", typeof(int));
            table.Columns.Add("OtherUnit", typeof(int));
            table.Columns.Add("EmptyUnit", typeof(int));
            table.Columns.Add("WaterInstallationDateJalali", typeof(string));
            table.Columns.Add("SewageInstallationDateJalali", typeof(string));
            table.Columns.Add("WaterRegisterDate", typeof(string));
            table.Columns.Add("SewageRegisterDate", typeof(string));
            table.Columns.Add("WaterCount", typeof(int));
            table.Columns.Add("SewageCalcState", typeof(int));
            table.Columns.Add("ContractualCapacity", typeof(int));
            table.Columns.Add("HouseholdNumber", typeof(int));
            table.Columns.Add("HouseholdDate", typeof(string));
            table.Columns.Add("VillageId", typeof(string));
            table.Columns.Add("IsSpecial", typeof(bool));
            table.Columns.Add("MeterDiameterId", typeof(short));
            table.Columns.Add("VirtualCategoryId", typeof(int));

            table.Columns.Add("TavizDateJalali", typeof(string));
            table.Columns.Add("TavizCause", typeof(string));
            table.Columns.Add("TavizRegisterDateJalali", typeof(string));
            table.Columns.Add("TavizNumber", typeof(int));

            table.Columns.Add("LastMeterDateJalali", typeof(string));
            table.Columns.Add("LastMeterNumber", typeof(int));
            table.Columns.Add("LastConsumption", typeof(float));
            table.Columns.Add("LastMonthlyConsumption", typeof(float));
            table.Columns.Add("LastCounterStateCode", typeof(int));

            table.Columns.Add("SumItems", typeof(double));
            table.Columns.Add("SumItemsBeforeDiscount", typeof(double));
            table.Columns.Add("DiscountSum", typeof(double));
            table.Columns.Add("Consumption", typeof(double));
            table.Columns.Add("MonthlyConsumption", typeof(double));

            foreach (var item in input)
            {
                var row = table.NewRow();

                row["FlowImportedId"] = item.FlowImportedId;
                row["ZoneId"] = item.ZoneId;
                row["CustomerNumber"] = item.CustomerNumber;
                row["ReadingNumber"] = item.ReadingNumber;
                row["BillId"] = item.BillId;
                row["AgentCode"] = item.AgentCode;
                row["CurrentCounterStateCode"] = item.CurrentCounterStateCode;
                row["PreviousDateJalali"] = item.PreviousDateJalali;
                row["CurrentDateJalali"] = item.CurrentDateJalali;
                row["PreviousNumber"] = item.PreviousNumber;
                row["CurrentNumber"] = item.CurrentNumber;

                row["InsertByUserId"] = item.InsertByUserId;
                row["InsertDateTime"] = item.InsertDateTime;
                row["RemovedByUserId"] = item.RemovedByUserId ?? (object)DBNull.Value;
                row["RemovedDateTime"] = item.RemovedDateTime ?? (object)DBNull.Value;
                row["ExcludedByUserId"] = item.ExcludedByUserId ?? (object)DBNull.Value;
                row["ExcludedDateTime"] = item.ExcludedDateTime ?? (object)DBNull.Value;

                row["UsageId"] = item.UsageId;
                row["DomesticUnit"] = item.DomesticUnit;
                row["CommercialUnit"] = item.CommercialUnit;
                row["OtherUnit"] = item.OtherUnit;
                row["EmptyUnit"] = item.EmptyUnit;
                row["WaterInstallationDateJalali"] = item.WaterInstallationDateJalali;
                row["SewageInstallationDateJalali"] = item.SewageInstallationDateJalali;
                row["WaterRegisterDate"] = item.WaterRegisterDate;
                row["SewageRegisterDate"] = item.SewageRegisterDate;
                row["WaterCount"] = item.WaterCount;
                row["SewageCalcState"] = item.SewageCalcState;
                row["ContractualCapacity"] = item.ContractualCapacity;
                row["HouseholdNumber"] = item.HouseholdNumber;
                row["HouseholdDate"] = item.HouseholdDate;
                row["VillageId"] = item.VillageId ?? (object)DBNull.Value;
                row["IsSpecial"] = item.IsSpecial;
                row["MeterDiameterId"] = item.MeterDiameterId;
                row["VirtualCategoryId"] = item.VirtualCategoryId;

                row["TavizDateJalali"] = item.TavizDateJalali ?? (object)DBNull.Value;
                row["TavizCause"] = item.TavizCause ?? (object)DBNull.Value;
                row["TavizRegisterDateJalali"] = item.TavizRegisterDateJalali ?? (object)DBNull.Value;
                row["TavizNumber"] = item.TavizNumber ?? (object)DBNull.Value;

                row["LastMeterDateJalali"] = item.LastMeterDateJalali;
                row["LastMeterNumber"] = item.LastMeterNumber ?? (object)DBNull.Value;
                row["LastConsumption"] = item.LastConsumption ?? (object)DBNull.Value;
                row["LastMonthlyConsumption"] = item.LastMonthlyConsumption ?? (object)DBNull.Value;
                row["LastCounterStateCode"] = item.LastCounterStateCode ?? (object)DBNull.Value;

                row["SumItems"] = item.SumItems ?? (object)DBNull.Value;
                row["SumItemsBeforeDiscount"] = item.SumItemsBeforeDiscount ?? (object)DBNull.Value;
                row["DiscountSum"] = item.DiscountSum ?? (object)DBNull.Value;
                row["Consumption"] = item.Consumption ?? (object)DBNull.Value;
                row["MonthlyConsumption"] = item.MonthlyConsumption ?? (object)DBNull.Value;

                table.Rows.Add(row);
            }
            return table;
        }
        public DataTable ToDataTable(IEnumerable<MeterReadingWithAbBahaResultUpdateDto> items)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("SumItems", typeof(double));
            dt.Columns.Add("SumItemsBeforeDiscount", typeof(double));
            dt.Columns.Add("DiscountSum", typeof(double));
            dt.Columns.Add("Consumption", typeof(double));
            dt.Columns.Add("MonthlyConsumption", typeof(double));

            foreach (var item in items)
            {
                dt.Rows.Add(
                    item.Id,
                    item.SumItems,
                    item.SumItemsBeforeDiscount,
                    item.DiscountSum,
                    item.Consumption,
                    item.MonthlyConsumption
                );
            }

            return dt;
        }

        private string GetQuery()//Todo : remove top 100
        {
            return @"Select top 100 *
                        From Atlas.dbo.MeterReadingDetail
                        Where FlowImportedId=@flowImportedId";
        }

    }
}