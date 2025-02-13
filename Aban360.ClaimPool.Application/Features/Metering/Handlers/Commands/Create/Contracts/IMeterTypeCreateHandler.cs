using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts
{
    public interface IMeterTypeCreateHandler
    {
        Task Handle(MeterTypeCreateDto createDto, CancellationToken cancellationToken);
    }
}
