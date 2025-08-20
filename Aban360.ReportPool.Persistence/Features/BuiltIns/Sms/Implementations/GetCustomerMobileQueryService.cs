using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
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
            GetCustomerMobileOutputDto mobileData = await _sqlReportConnection.QueryFirstOrDefaultAsync<GetCustomerMobileOutputDto>(customerMobileQuery, new { billId = input.BillId });
            if (mobileData == null)
            {
                throw new BaseException(ExceptionLiterals.NotFoundPhoneNumber);
            }
            return mobileData;
        }
        private string GetCustomerMobile()
        {
            return @"Select
                    	c.MobileNo AS Mobile
                    From [CustomerWarehouse].dbo.Clients c
                    where 
                        c.BillId=@billId AND
						c.ToDayJalali IS NULL";
        }
    }
}
#region
/*
Select Top 1
	c.MobileNo AS Mobile
From [CustomerWarehouse].dbo.Clients c
where c.BillId=@billId*/
#endregion