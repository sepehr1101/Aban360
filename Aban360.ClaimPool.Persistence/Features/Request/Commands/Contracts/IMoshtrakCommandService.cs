using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;

namespace Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts
{
    public interface IMoshtrakCommandService
    {
        Task Update(MoshtrkUpdateDto input);
    }
}
