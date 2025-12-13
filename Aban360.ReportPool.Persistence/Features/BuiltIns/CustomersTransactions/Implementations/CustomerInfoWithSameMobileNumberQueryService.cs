using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class CustomerInfoWithSameMobileNumberQueryService : AbstractBaseConnection, ICustomerInfoWithSameMobileNumberQueryService
    {
        public CustomerInfoWithSameMobileNumberQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<CustomerInfoWithSameMobileNumberHeaderOutputDto, CustomerInfoWithSameMobileNumberDataOutputDto>> GetInfo(string mobileNumber)
        {
            string query = GetQuery();

            IEnumerable<CustomerInfoWithSameMobileNumberDataOutputDto> data = await _sqlReportConnection.QueryAsync<CustomerInfoWithSameMobileNumberDataOutputDto>(query, new { MobileNo = mobileNumber });
            CustomerInfoWithSameMobileNumberHeaderOutputDto header = new CustomerInfoWithSameMobileNumberHeaderOutputDto()
            {
                MobileNumber = mobileNumber,
                RecordCount = data is not null && data.Any() ? data.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
            };

            var result = new ReportOutput<CustomerInfoWithSameMobileNumberHeaderOutputDto, CustomerInfoWithSameMobileNumberDataOutputDto>(ReportLiterals.CustomerInfoWithSamePhoneNumber, header, data);

            return result;
        }
        private string GetQuery()
        {
            return @$"Select 
                    	ZoneId,
                    	ZoneTitle,
                    	BillId,
                    	FirstName,
                    	SureName as Surname,
                    	DeletionStateId,
                    	DeletionStateTitle
                    From CustomerWarehouse.dbo.Clients
                    Where 
                    	ToDayJalali IS NULL AND
                    	MobileNo=@MobileNo";
        }
    }
}
