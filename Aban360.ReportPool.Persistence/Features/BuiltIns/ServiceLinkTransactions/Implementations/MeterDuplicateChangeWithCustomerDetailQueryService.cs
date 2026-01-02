using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class MeterDuplicateChangeWithCustomerDetailQueryService : UseStateBase, IMeterDuplicateChangeWithCustomerDetailQueryService
    {
        public MeterDuplicateChangeWithCustomerDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<MeterDuplicateChangeWithCustomerDetailDataOutputDto>> Get(MeterDuplicateChangeWithCustomerInputDto input)
        {
            string query = GetQuery();
            IEnumerable<MeterDuplicateChangeWithCustomerDetailDataOutputDto> data = await _sqlReportConnection.QueryAsync<MeterDuplicateChangeWithCustomerDetailDataOutputDto>(query, input);

            return data;
        }

        private string GetQuery()
        {
            return @"Select 
                    	m.MeterNumber, 
                    	m.ChangeDateJalali ,
                    	m.RegisterDateJalali,
                    	m.ChangeCauseTitle,
                    	TRIM(m.BodySerial) BodySerial
                    From CustomerWarehouse.dbo.MeterChange m
                    Where 
                        ((@isRegisterDate = 1 AND m.ChangeDateJalali BETWEEN @fromDateJalali AND @toDateJalali) OR
                    	(@isRegisterDate<>1 AND m.ChangeDateJalali BETWEEN @fromDateJalali AND @toDateJalali)) AND
                        m.ZoneId=@zoneId AND m.CustomerNumber=@customerNumber";
        }
    }
}