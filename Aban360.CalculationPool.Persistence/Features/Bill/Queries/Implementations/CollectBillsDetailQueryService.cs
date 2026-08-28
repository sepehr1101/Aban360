using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
using Aban360.CalculationPool.Domain.Features.CollectBills.Outputs;
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
            IEnumerable<CollectBillsDetailGetDto> data = await _sqlReportConnection.QueryAsync<CollectBillsDetailGetDto>(query);

            return data;
        }
        public async Task<IEnumerable<CollectBillsDetailWithLastStepDataOutputDto>> Get(CollectBillsDetialReportInputDto input)
        {
            string query = GetReportQuery();
            IEnumerable<CollectBillsDetailWithLastStepDataOutputDto> result = await _sqlReportConnection.QueryAsync<CollectBillsDetailWithLastStepDataOutputDto>(query, input);
            return result;
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
        private string GetReportQuery()
        {
            return $@";With Cte As
                    (
                    	Select 
                    		*,
                    		Rn=ROW_NUMBER() OVER(Partition By  GroupingId Order By InsertDateTime Asc)
                    	From [Atlas].dbo.CollectBillsDetail
                    )
                    Select 
                    	firstStep.id FirstId,
                    	lastStep.id LastId,
                    	firstStep.GroupingId,
                    	firstStep.StepId FirstStepId,
                    	lastStep.stepId LastStepId,
						lastStep.FileName FileName,
                    	firstStep.InsertDateTime FirstStepInsertDateTime,
                    	lastStep.InsertDateTime LastStepInserteDateTime,
                    	lastStep.FinishDateTime LastStepFinishedDateTime,
                    	lastStep.Description LastStepDescription
                    From Cte firstStep
                    OUTER APPLY
                    (
                    	Select TOP 1 * 
                    	From [Atlas].dbo.CollectBillsDetail lastStep
                    	Where lastStep.GroupingId=firstStep.GroupingId 
                    	Order By InsertDateTime Desc
                    )as lastStep
                    Where 
                        Rn=1 AND
	                    Format(firstStep.InsertDateTime , 'yyyy/MM/dd' , 'fa') BETWEEN @FromDateJalali AND @ToDateJalali
                    Order By firstStep.InsertDateTime Desc";
        }
    }
}
