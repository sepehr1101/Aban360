using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts
{
    public interface IMotherUpdateHandler
    {
        Task Handle(MotherChildUpdateInputDto inputDto, CancellationToken cancellationToken);
        Task Handle(CommonSiphonUpdateInputDto inputDto, CancellationToken cancellationToken);
    }
}
