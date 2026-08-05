using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Implementations
{
    public sealed class CollectBillsDetailQueryService : AbstractBaseConnection, ICollectBillsDetailQueryService
    {
        public CollectBillsDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<CollectBillsDetailGetDto> Get(int id)
        {
            string query = GetByIdQuery();
            CollectBillsDetailGetDto data = await _sqlReportConnection.QueryFirstOrDefaultAsync<CollectBillsDetailGetDto>(query, new { id });

            return data;
        }
        public async Task<IEnumerable<CollectBillsDetailGetDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<CollectBillsDetailGetDto> data = await _sqlReportConnection.QueryAsync<CollectBillsDetailGetDto>(query, new { id });

            return data;
        }

        private string GetQuery()
        {
            return @"Select 
                    	cd.Id,
                    	cd.GroupingId,
                    	cd.StepId,
                    	cp.Title StepTitle,
                    	cp.[Order] StepOrder,
                    	cd.InsertDateTime,
                    	cd.FinishDateTime
                    From Atlas.dbo.CollectBillsDetail cd
                    Join Atlas.dbo.CollectBillsStep cp
                    	ON cd.StepId=cp.Id";
        }
        private string GetByIdQuery()
        {
            return @"Select 
                    	cd.Id,
                    	cd.GroupingId,
                    	cd.StepId,
                    	cp.Title StepTitle,
                    	cp.[Order] StepOrder,
                    	cd.InsertDateTime,
                    	cd.FinishDateTime
                    From Atlas.dbo.CollectBillsDetail cd
                    Join Atlas.dbo.CollectBillsStep cp
                    	ON cd.StepId=cp.Id
                    Where cd.Id = @Id";
        }
    }
}
