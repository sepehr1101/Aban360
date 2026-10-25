using Aban360.Common.ApplicationUser;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts
{
    public interface INowGenerateBillHandler
    {
        Task<NowGenerateBillOutputDto> Handle(NowGenerateBillInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
