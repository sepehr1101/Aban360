using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using LiteDB;
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

        public async Task<IEnumerable<MeterLifeOutputDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<MeterLifeOutputDto> result = await _sqlReportConnection.QueryAsync<MeterLifeOutputDto>(query);

            return result;
        }
        public async Task Create(IEnumerable<MeterLifeOutputDto> input)
        {
            var dataTable = ToDataTable(input);

            using (var sqlConnection = _sqlReportConnection)
            {
                await sqlConnection.OpenAsync();

                using (var bulkCopy = new SqlBulkCopy(sqlConnection))
                {
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
            }
        }
        private DataTable ToDataTable(IEnumerable<MeterLifeOutputDto> input)
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

        private string GetQuery()
        {
            return @"Select 
                    	c.ZoneId,
                    	c.ZoneTitle,
                    	c.CustomerNumber,
                    	c.BillId,
                    	c.UsageStateId as BranchTypeId,
                    	c.UsageId,
                    	c.UsageTitle,
                    	c.WaterInstallDate as WaterInstallationDateJalali,
                    	m.ChangeDateJalali as LatestChangeDataJalali
                    From CustomerWarehouse.dbo.Clients c
                    Left Join CustomerWarehouse.dbo.MeterChange m
                    	ON c.ZoneId=m.ZoneId AND c.CustomerNumber=m.CustomerNumber
                    Where
                    	c.ToDayJalali IS NULL 
                        AND c.ZoneId = 131301
                    Order By m.ChangeDateJalali Desc";
        }
        private string GetInsertCommand()
        {
            return @"Insert Into [CustomerWarehouse].dbo.MeterLife(
                        ZoneId,ZoneTitle,
                        CustomerNumber,BillId,
                        BranchTypeId,UsageId,UsageTitle,
                        LifeInDay,LifeText)
                    Values(
                        @ZoneId,@ZoneTitle,
                        @CustomerNumber,@BillId,
                        @BranchTypeId,@UsageId,@UsageTitle,
                        @LifeInDay,@LifeText)";
        }
    }
}
