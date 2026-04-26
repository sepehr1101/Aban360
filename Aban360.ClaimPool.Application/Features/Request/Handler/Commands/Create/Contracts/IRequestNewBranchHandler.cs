using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts
{
    public interface IRequestNewBranchHandler
    {
        Task<MoshtrakCreateDto> Handle(RequestNewBranchInputDto inputDto, int userName, CancellationToken cancellationToken);
    }
}
