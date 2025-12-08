using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts
{
    public interface IBillQueryService
    {
        Task<BedBesConsumptionOutputDto> Get(string billId);
        Task<BedBesDataInfoOutptuDto> Get(int id);
        Task<IEnumerable<BillsCanRemovedOutputDto>> GetToRemove(RemovedBillSearchDto input);
        Task<RemoveBillInputDto> GetToRemove(int id);
        Task<IEnumerable<BillsCanRemovedOutputDto>> GetToReturned(ReturnedBillSearchDto input);
        Task<IEnumerable<BillsCanRemovedOutputDto>> Get(BillToReturnInputDto input);
    }
}
