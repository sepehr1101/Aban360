using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Delete.Contracts
{
    public interface ICalculationRequestRemoveManualHandler
    {
        Task Handle(KartRemoveManualInputDto inputDto, CancellationToken cancellationToken);
    }
}
