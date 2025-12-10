using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;

namespace Aban360.TaxPool.Persistence.Features.MaaherTSP.Contracts
{
    public interface IMaliatMaaherWrapperService
    {
        Task<int> Insert(MaliatMaaherWrapperInsertDto input);
        Task UpdateAmountAndCount(MaliatMaaherWrapperAmountAndCountUpdateDto input);
        Task UpdateConfirmed(MaliatMaaherWrapperConfirmedUpdateDto input);
        Task UpdateSend(MaliatMaaherWrapperSendUpdateDto input);
        Task<MaliatMaaherWrapperGetDto> Get(int id);
        Task<IEnumerable<MaliatMaaherWrapperGetDto>> Get();
    }
}
