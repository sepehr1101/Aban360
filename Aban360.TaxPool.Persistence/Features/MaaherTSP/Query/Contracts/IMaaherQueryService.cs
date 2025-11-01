using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.RecieveDto;

namespace Aban360.TaxPool.Persistence.Features.MaaherTSP.Query.Contracts
{
    public interface IMaaherQueryService
    {
        Task<MaaherErrorsDto> GetErrors(int errorCode);
    }
}
