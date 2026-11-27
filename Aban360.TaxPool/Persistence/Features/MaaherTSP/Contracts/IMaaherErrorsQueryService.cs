using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;

namespace Aban360.TaxPool.Persistence.Features.MaaherTSP.Contracts
{
    public interface IMaaherErrorsQueryService
    {
        Task<MaaherErrorsDto> GetErrors(int errorCode);
    }
}
