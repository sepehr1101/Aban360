using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
using Aban360.CalculationPool.Domain.Features.CollectBills.Outputs;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface ICollectBillsDetailQueryService
    {
        Task<CollectBillsDetailGetDto> Get(int id);
        Task<IEnumerable<CollectBillsDetailGetDto>> Get();
        Task<IEnumerable<CollectBillsDetailWithLastStepDataOutputDto>> Get(CollectBillsDetialReportInputDto input);
    }
}
