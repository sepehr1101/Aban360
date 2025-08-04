using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class CustomerOldInfoQueryService : AbstractBaseConnection, ICustomerOldInfoQueryService
    {
        public CustomerOldInfoQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<CustomerOldInfoOutputDto> GetInfo(CustomerOldInfoInputDto input)
        {
            string customerOldInfoQuery = GetCustomerOldInfoQuery();
            var @params = new
            {
                zoneId = input.ZoneId,
                customerNumber = input.CustomerNumber,
            };
            CustomerOldInfoOutputDto? output = await _sqlReportConnection.QueryFirstOrDefaultAsync<CustomerOldInfoOutputDto>(customerOldInfoQuery, @params);
            return output;
        }

        private string GetCustomerOldInfoQuery()
        {
            return @"USE [CustomerWarehouse] 
                     SELECT  
                        ZoneId, 
                        ZoneTitle, 
                        CustomerNumber, 
                        OldCustomerNumber, 
                        BillId, 
                        OldBillId, 
                        FirstName, 
                        SureName AS Surname,
                        VillageId,
                        VillageName,
                        m.C0 RegionId,
						m.C2 RegionTitle
                     FROM Clients c
                     LEFT OUTER JOIN Db70.dbo.T51 z
						 ON c.ZoneId=Z.c0
					 LEFT OUTER JOIN Db70.dbo.T46 m
						ON z.C1=m.c0
                     WHERE 
                        ZoneId=@zoneId AND  
                        (CustomerNumber=@customerNumber OR OldCustomerNumber=@customerNumber) AND 
                        ToDayJalali IS NULL";
        }
    }
}
