using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class CustomerMobileListService : AbstractBaseConnection, ICustomerMobileListService
    {
        public CustomerMobileListService(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<IEnumerable<BillIdMobileDto>> GetCustomerMobileInfo(BillIdListDtoWrapper billIdListDtoWrapper)
        {
            string query = GetBillIdMobileQuery();
            List<string> billIds = billIdListDtoWrapper.BillIdList
                .Select(b => b.BillId)
                .ToList();
            IEnumerable<BillIdMobileDto> billIdMobileDtos = await _sqlConnection.QueryAsync<BillIdMobileDto>(query, new { BillIds = billIds });
            return billIdMobileDtos;
        }
        private string GetBillIdMobileQuery()
        {
            string query = @"SELECT [BillId], [MobileNo]
                            FROM [172.18.12.60].CustomerWarehouse.[dbo].[BillIdMobileNo]
                               WHERE ([BillId]) in @BillIds";
            return query;
        }
    }
}
