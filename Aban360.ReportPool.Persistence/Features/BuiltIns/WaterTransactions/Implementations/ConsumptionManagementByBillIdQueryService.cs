using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class ConsumptionManagementByBillIdQueryService : AbstractBaseConnection, IConsumptionManagementByBillIdQueryService
    {
        public ConsumptionManagementByBillIdQueryService(IConfiguration configuration)
                : base(configuration)
        {
        }

        public async Task<IEnumerable<ConsumptionManagementByBillIdGetDto>> Get(ConsumptionManagementByBillIdDto inputDto)
        {
            string query = GetQuery(GetDbName(inputDto.ZoneId));
            IEnumerable<ConsumptionManagementByBillIdGetDto> data = await _sqlReportConnection.QueryAsync<ConsumptionManagementByBillIdGetDto>(query, inputDto);
            return data;
        }
        private string GetQuery(string dbName)
        {
            return $@"Select 
                    	b.id Id,
                    	b.town ZoneId,
                    	b.radif CustomerNumber,
                    	b.sh_ghabs1 BillId,
                    	b.edareh_k ReadingNumber,
                    	b.pri_date PreviousDateJalali,
                    	b.pri_no PreviousNumber,
                    	b.today_date CurrentDateJalali,
                    	b.today_no CurrentNumber,
                    	b.modat Duration,
                    	b.masraf Consumption,
                    	b.rate ConsumptionAverage,
                    	b.cod_enshab UsageId,
                    	t41.C1 UsageTitle
                    From [{dbName}].dbo.bed_bes b
                    Join [Db70].dbo.T41 t41
                    	ON b.cod_enshab=t41.C0
                    Where 
                    	b.radif=@CustomerNumber AND
                    	b.today_date>@FromDateJalali AND b.pri_date<@ToDateJalali
                    Order by date_bed Asc";
        }
    }
}
