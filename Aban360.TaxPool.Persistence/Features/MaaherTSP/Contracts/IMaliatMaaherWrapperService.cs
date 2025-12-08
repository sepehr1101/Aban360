using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;

namespace Aban360.TaxPool.Persistence.Features.MaaherTSP.Contracts
{
    public interface IMaliatMaaherWrapperService
    {
        Task<int> Insert(MaliatMaaherWrapperInsertDto input);
        Task UpdateAmountAndCount(UpdateMaliatMaaherWrapperAmountAndCountDto input);
    }
}
