using Aban360.OldCalcPool.Domain.Constants;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts
{
    public interface INowGenerateBillRequestService
    {
        Task<NowGenerateBillErrorCodeEnum> Register(NowGenerateBillInputDto input, CancellationToken cancellationToken);
    }
}
