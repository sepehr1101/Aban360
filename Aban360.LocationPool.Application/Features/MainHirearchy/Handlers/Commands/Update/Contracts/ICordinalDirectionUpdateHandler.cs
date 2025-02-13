using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Update.Contracts
{
    public interface ICordinalDirectionUpdateHandler
    {
        Task Handle(CordinalDirectionUpdateDto updateDto,CancellationToken cancellationToken);
    }
}
