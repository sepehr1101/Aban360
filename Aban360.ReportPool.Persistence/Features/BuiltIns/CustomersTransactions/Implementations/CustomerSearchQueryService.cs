using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class CustomerSearchQueryService : AbstractBaseConnection, ICustomerSearchQueryService
    {
        public CustomerSearchQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<CustomerSearchHeaderOutputDto, CustomerSearchDataOutputDto>> GetInfo(CustomerSearchInputDto input)
        {
            string customerSearchDataInfoQuery = GetCustomerSearchDataQuery();

            var param = new { input = $"%{input.InputText}%" };
            IEnumerable<CustomerSearchDataOutputDto> customerData = await _sqlReportConnection.QueryAsync<CustomerSearchDataOutputDto>(customerSearchDataInfoQuery, param,null, 120);
            CustomerSearchHeaderOutputDto customerHeader = new CustomerSearchHeaderOutputDto()
            { 
                RecordCount=customerData.Count()
            };

            var result = new ReportOutput<CustomerSearchHeaderOutputDto, CustomerSearchDataOutputDto>(ReportLiterals.CustomerSearch, customerHeader, customerData);
            return result;
        }

        private string GetCustomerSearchDataQuery()
        {
            return @"select TOP(100)
                        c.CustomerNumber,
                      	c.ReadingNumber,
                      	c.FirstName,
                      	c.SureName AS Surname,
                      	c.WaterDiameterTitle AS MeterDiameterTitle,
                      	c.BillId,
                      	c.DomesticCount AS DomesticUnit,
                      	c.CommercialCount AS CommercialUnit,
                      	c.OtherCount AS OtherUnit,
                      	c.MobileNo AS MobileNumber,
                      	c.Address
                      from [CustomerWarehouse].dbo.Clients c
                      where 
                            c.ToDayJalali is null AND
                            (
                                c.CustomerNumber like @input 
                                or c. ReadingNumber like @input
                                or c.FirstName like @input
                                or c.SureName like @input
                                or c.WaterDiameterId like @input
                                or c.BillId like @input
                                or c.DomesticCount like @input
                                or c.CommercialCount like @input
                                or c.OtherCount like @input
                                or c.MobileNo like @input
                                or c.Address like @input
                            )";
        }
    }
}
