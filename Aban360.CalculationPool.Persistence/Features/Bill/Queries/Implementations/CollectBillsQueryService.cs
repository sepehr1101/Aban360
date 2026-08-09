using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Implementations
{
    public sealed class CollectBillsQueryService : AbstractBaseConnection, ICollectBillsQueryService
    {
        public CollectBillsQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IEnumerable<CollectBillsDataDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<CollectBillsDataDto> data = await _sqlReportConnection.QueryAsync<CollectBillsDataDto>(query, new { CurrentDateJalali =DateTime.Now.ToShortPersianDateString()});

            return data;
        }
        private string GetQuery()
        {
            return @"Select Top 10 
                    	CONCAT(ZoneTitle,'-',BillId Collate Arabic_CI_AS,'-',CounterStateTitle,'-',Consumption,'-',Duration) Text
                    From CustomerWarehouse.dbo.Bills
                    Where RegisterDay=@CurrentDateJalali";
        }
    }
}
