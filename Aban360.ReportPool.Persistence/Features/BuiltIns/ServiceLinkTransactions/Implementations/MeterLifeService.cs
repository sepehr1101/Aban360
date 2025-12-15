using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class MeterLifeService : AbstractBaseConnection, IMeterLifeService
    {
        public MeterLifeService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task TruncateTable()
        {
            string query = GetTruncateQuery();
            await _sqlReportConnection.ExecuteAsync(query);
        }
        public async Task<ReportOutput<MeterLifeHeaderOutputDto, MeterLifeDataOutputDto>> Get(MeterLifeInputDto input)
        {
            string query = GetFromMeterLifeQuery();
            IEnumerable<MeterLifeDataOutputDto> data = await _sqlReportConnection.QueryAsync<MeterLifeDataOutputDto>(query, input);
            MeterLifeHeaderOutputDto header = new MeterLifeHeaderOutputDto()
            {
                FromLifeInDay = input.FromLifeInDay/365 ?? 0,
                ToLifeInDay = input.ToLifeInDay/365 ?? 0,

                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = data.Count(),
                CustomerCount = data.Count(),
                Title = ReportLiterals.MeterLifeDetail,
            };
            if (data is not null && data.Count() > 0)
            {
                header.AverageLifeInDay = (int)data.Where(m => m.LifeInDay != -1).Average(m => m.LifeInDay);
                header.MaxLifeInDay = (int)data.Max(m => m.LifeInDay);
                header.MinLifeInDay = (int)data.Where(m => m.LifeInDay != -1).Min(m => m.LifeInDay);
                header.IncalculableCount = (int)data.Where(m => m.LifeInDay == -1).Count();
            }

            ReportOutput<MeterLifeHeaderOutputDto, MeterLifeDataOutputDto> result = new(ReportLiterals.MeterLifeDetail, header, data);
            return result;
        }
        public async Task<ReportOutput<MeterLifeSummaryHeaderOutputDto, MeterLifeSummaryDataOutputDto>> GetSummary(MeterLifeInputDto input)
        {
            string query = GetSummaryQuery();
            IEnumerable<MeterLifeSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<MeterLifeSummaryDataOutputDto>(query, input);
            MeterLifeSummaryHeaderOutputDto header = new MeterLifeSummaryHeaderOutputDto()
            {
                FromLifeInDay = input.FromLifeInDay ?? 0,
                ToLifeInDay = input.ToLifeInDay ?? 0,

                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = data.Count(),
                CustomerCount = data is not null && data.Count() > 0 ? data.Sum(m => m.CustomerCount) : 0,
                Title = ReportLiterals.MeterLifeDetail,
            };

            ReportOutput<MeterLifeSummaryHeaderOutputDto, MeterLifeSummaryDataOutputDto> result = new(ReportLiterals.MeterLifeSummary, header, data);
            return result;
        }
        public async Task<IEnumerable<MeterLifeCalculationOutputDto>> GetFromClient()
        {
            string query = GetFromClientTableQuery();
            IEnumerable<MeterLifeCalculationOutputDto> result = await _sqlReportConnection.QueryAsync<MeterLifeCalculationOutputDto>(query);

            return result;
        }
        public async Task Insert(IEnumerable<MeterLifeCalculationOutputDto> input)
        {
            var dataTable = GetDataTable(input);
            using SqlConnection sqlConnection = new (_sqlReportConnection.ConnectionString);
            await sqlConnection.OpenAsync();
            using SqlBulkCopy bulkCopy = new (sqlConnection);
            bulkCopy.DestinationTableName = "[CustomerWarehouse].dbo.MeterLife";

            bulkCopy.ColumnMappings.Add("ZoneId", "ZoneId");
            bulkCopy.ColumnMappings.Add("ZoneTitle", "ZoneTitle");
            bulkCopy.ColumnMappings.Add("CustomerNumber", "CustomerNumber");
            bulkCopy.ColumnMappings.Add("BillId", "BillId");
            bulkCopy.ColumnMappings.Add("BranchTypeId", "BranchTypeId");
            bulkCopy.ColumnMappings.Add("UsageId", "UsageId");
            bulkCopy.ColumnMappings.Add("UsageTitle", "UsageTitle");
            bulkCopy.ColumnMappings.Add("LifeInDay", "LifeInDay");
            bulkCopy.ColumnMappings.Add("LifeText", "LifeText");

            bulkCopy.BatchSize = 10000;
            bulkCopy.BulkCopyTimeout = 0;

            await bulkCopy.WriteToServerAsync(dataTable);
        }
        private DataTable GetDataTable(IEnumerable<MeterLifeCalculationOutputDto> input)
        {
            var table = new DataTable();

            table.Columns.Add("ZoneId", typeof(int));
            table.Columns.Add("ZoneTitle", typeof(string));
            table.Columns.Add("CustomerNumber", typeof(string));
            table.Columns.Add("BillId", typeof(string));
            table.Columns.Add("BranchTypeId", typeof(int));
            table.Columns.Add("UsageId", typeof(int));
            table.Columns.Add("UsageTitle", typeof(string));
            table.Columns.Add("LifeInDay", typeof(int));
            table.Columns.Add("LifeText", typeof(string));

            foreach (var x in input)
            {
                table.Rows.Add(
                    x.ZoneId, x.ZoneTitle,
                    x.CustomerNumber, x.BillId,
                    x.BranchTypeId, x.UsageId, x.UsageTitle,
                    x.LifeInDay, x.LifeText
                );
            }

            return table;
        }

        private string GetFromClientTableQuery()
        {
            return @"SELECT 
                        c.ZoneId,
                        c.ZoneTitle,
                        c.CustomerNumber,
                        c.BillId,
                        c.UsageStateId AS BranchTypeId,
                        c.UsageId,
                        c.UsageTitle,
                        c.WaterInstallDate AS WaterInstallationDateJalali,
                        m.LatestChangeDateJalali
                    FROM CustomerWarehouse.dbo.Clients c
                    OUTER APPLY
                    (
                        SELECT TOP (1) ChangeDateJalali AS LatestChangeDateJalali
                        FROM CustomerWarehouse.dbo.MeterChange mc
                        WHERE mc.ZoneId = c.ZoneId
                          AND mc.CustomerNumber = c.CustomerNumber
                        ORDER BY mc.ChangeDateJalali DESC
                    ) m
                    WHERE
                        c.ToDayJalali IS NULL AND
                        c.DeletionStateId NOT IN (1,2)
                    ORDER BY m.LatestChangeDateJalali DESC;";
        }
        private string GetFromMeterLifeQuery()
        {
            return @"Select *
                    From CustomerWarehouse.dbo.MeterLife
                    Where 
                    	ZoneId IN @zoneIds AND
                    	UsageId IN @usageIds AND
	                    (@FromLifeInDay IS NULL OR
	                    @ToLifeInDay IS NULL OR
	                    LifeInDay BETWEEN @FromLifeInDay AND @ToLifeInDay)";
        }
        private string GetTruncateQuery()
        {
            string query = "TRUNCATE TABLE CustomerWarehouse.dbo.MeterLife;";
            return query;
        }
        private string GetSummaryQuery()
        {
            return @"Select
                    	MAX(t46.C2) AS RegionTitle,
                    	m.ZoneTitle,
	                    Count(m.ZoneTitle) as CustomerCount,
                    	Count(Case When m.LifeInDay<=1825 Then 1 Else null End) as LessThan5Year,
                    	Count(Case When m.LifeInDay>1825 AND m.LifeInDay<=3650 Then 1 Else null End) as Between5_10Year,
                    	Count(Case When m.LifeInDay>3650 AND m.LifeInDay<=5475 Then 1 Else null End) as Between10_15Year,
                    	Count(Case When m.LifeInDay>5475 AND m.LifeInDay<=7300 Then 1 Else null End) as Between15_20Year,
                    	Count(Case When m.LifeInDay>7300 AND m.LifeInDay<=9125 Then 1 Else null End) as Between20_25Year,
                    	Count(Case When m.LifeInDay>9125 AND m.LifeInDay<=10950 Then 1 Else null End) as Between25_30Year,
                    	Count(Case When m.LifeInDay>10950 AND m.LifeInDay<=12775 Then 1 Else null End) as Between30_35Year,
                    	Count(Case When m.LifeInDay>12775 AND m.LifeInDay<=14600 Then 1 Else null End) as Between35_40Year,
                    	Count(Case When m.LifeInDay>14600 AND m.LifeInDay<=16425 Then 1 Else null End) as Between40_45Year,
                    	Count(Case When m.LifeInDay>16425 Then 1 Else null End) as MoreThan45Year
                    From CustomerWarehouse.dbo.MeterLife m
                    Join [Db70].dbo.T51 t51
                    	On t51.C0=m.ZoneId
                    Join [Db70].dbo.T46 t46
                    	On t51.C1=t46.C0
                    Where 
                       	ZoneId IN @zoneIds AND
                       	UsageId IN @usageIds AND
                        (@FromLifeInDay IS NULL OR
                        @ToLifeInDay IS NULL OR
                        LifeInDay BETWEEN @FromLifeInDay AND @ToLifeInDay)
                    GROUP By m.ZoneTitle";
        }
    }
}
