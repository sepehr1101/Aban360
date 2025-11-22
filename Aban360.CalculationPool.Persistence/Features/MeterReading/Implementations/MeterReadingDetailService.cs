using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.Db.Dapper;
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
        private DataTable ToDataTable(IEnumerable<MeterReadingDetailCreateDto> input)
        {
            var table=new DataTable();

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
            table.Columns.Add("ConsumptionAverage", typeof(float));
            table.Columns.Add("LastCounterStateCode", typeof(int));

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
                row["TavizCause"] = item.TavizCause?? (object)DBNull.Value;
                row["TavizRegisterDateJalali"] = item.TavizRegisterDateJalali??(object)DBNull.Value;
                row["TavizNumber"] = item.TavizNumber ?? (object)DBNull.Value;

                row["LastMeterDateJalali"] = item.LastMeterDateJalali;
                row["LastMeterNumber"] = item.LastMeterNumber ?? (object)DBNull.Value;
                row["ConsumptionAverage"] = item.ConsumptionAverage ?? (object)DBNull.Value;
                row["LastCounterStateCode"] = item.LastCounterStateCode ?? (object)DBNull.Value;

                table.Rows.Add(row);
            }
            return table;
        }
    }
}