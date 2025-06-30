using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.Sms.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.Sms.Implementations
{
    internal sealed class GetCustomerMobileQueryService : AbstractBaseConnection, IGetCustomerMobileQueryService
    {
        public GetCustomerMobileQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<GetCustomerMobileOutputDto> Get(GetCustomerMobileInputDto input)
        {
            string customerMobileQuery = GetCustomerMobile();
            GetCustomerMobileOutputDto mobileData = await _sqlReportConnection.QueryFirstAsync<GetCustomerMobileOutputDto>(customerMobileQuery, new { billId = input.BillId });
            return mobileData;
        }
        private string GetCustomerMobile()
        {
            return @"Select Top 1
                    	c.MobileNo AS Mobile
                    From [CustomerWarehouse].dbo.Clients c
                    where c.BillId=@billId";
        }
    }
}
