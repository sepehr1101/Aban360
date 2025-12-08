using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;

namespace Aban360.TaxPool.Persistence.Features.MaaherTSP.Contracts
{
    public interface IMaliatMaaherDetailService
    {
        Task Inserts(IEnumerable<MaliatMaaherDetailGetDto> items);
        Task<IEnumerable<MaliatMaaherDetailGetDto>> Get(MaliatMaaherDetailInsertBatchDto input);
        Task<MaliatMaaherDetailAmountAndCountDto> Get(int wrapperId);
    }
}
