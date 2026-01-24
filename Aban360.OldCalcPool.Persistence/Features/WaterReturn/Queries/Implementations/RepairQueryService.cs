using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Implementations
{
    internal sealed class RepairQueryService : AbstractBaseConnection, IRepairQueryService
    {
        public RepairQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<RepairGetDto> Get(int id)
        {
            string query = GetQuery();

            RepairGetDto repair = await _sqlReportConnection.QueryFirstOrDefaultAsync<RepairGetDto>(query, new { id });

            return repair;
        }
        public async Task<IEnumerable<RepairGetDto>> Get(string billId)
        {
            int customerNumber = await GetCustomerNumber(billId);
            string query = GetByBillIdQuery();

            IEnumerable<RepairGetDto> repair = await _sqlReportConnection.QueryAsync<RepairGetDto>(query, new { customerNumber });

            return repair;
        }
        public async Task<int> GetRepairCount(ZoneIdAndCustomerNumberOutputDto input, int jalaseNumber)
        {
            //string dbName = GetDbName(input.ZoneId);
            string dbName = "Atlas";
            string query = GetRepairCountWithJalaseNumber(dbName);
            var @params = new
            {
                zoneId = input.ZoneId,
                customerNumber = input.CustomerNumber,
                jalaseNumber = jalaseNumber
            };
            int count = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(query, @params);
            return count;
        }

        private async Task<int> GetCustomerNumber(string billId)
        {
            string query = GetCustomerNumberQuery();
            int customerNumber = await _sqlConnection.QueryFirstOrDefaultAsync<int>(query, new { billId });

            return customerNumber;
        }
        private string GetQuery()
        {
            return @"Select * 
                    From [Atlas].dbo.REPAIR
                    Where Id=@Id";
        }
        private string GetByBillIdQuery()
        {
            return @$"Select * 
                    From [Atlas].dbo.REPAIR
                    Where radif=@customerNumber";
        }
        private string GetCustomerNumberQuery()
        {
            return @"Select CustomerNumber
                    From [CustomerWarehouse].dbo.Clients
                    Where 
                    	BillId=@billId AND
                    	ToDayJalali IS NULL";
        }
        private string GetRepairCountWithJalaseNumber(string dbName)
        {
            return $@"Select COUNT(1)
                    From Atlas.dbo.REPAIR
                    Where
                    	town=@zoneId AND
                    	radif=@customerNumber AND
                    	jalase_no=@jalaseNumber";
        }

        //public async Task<RepairGetDto> Get(string billId)
        //{
        //    int zoneId=await GetZoneId(billId);
        //    string dbName = GetDbName(zoneId);
        //    string query = GetQuery(dbName);

        //    RepairGetDto repair = await _sqlReportConnection.QueryFirstOrDefaultAsync<RepairGetDto>(query, new { billId });

        //    return repair;
        //}
        //private async Task<int> GetZoneId(string billId)
        //{
        //    string query = GetZoneIdQuery();
        //    int zoneId = await _sqlConnection.QueryFirstOrDefaultAsync<int>(query, new { billId = billId });

        //    return zoneId;
        //}
        //private string GetQuery(string dbName)
        //{
        //    return @$"Select * 
        //            From [{dbName}].dbo.REPAIR
        //            Where radif=@customerNumber";
        //}
        //private string GetZoneIdQuery()
        //{
        //    return @"Select ZoneId
        //            From [CustomerWarehouse].dbo.Clients
        //            Where 
        //            	BillId=@billId AND
        //            	ToDayJalali IS NULL";
        //}
    }
}